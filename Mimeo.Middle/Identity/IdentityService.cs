using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mimeo.Blocks.Logging;

namespace Mimeo.Middle.Identity
{
    public class IdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly MimeoLogger _mimeoLogger;

        public IdentityService(
                UserManager<ApplicationUser> userManager, 
                ApplicationDbContext dbContext, 
                MimeoLogger mimeoLogger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mimeoLogger = mimeoLogger;
        }


        // Instance management
        //
        public async Task<Instance> CreateInstanceAsync(string name, string database, bool isEnable)
        {
            var instance = new Instance();
            instance.InstanceName = name;
            instance.Database = database;
            instance.IsEnabled = isEnable;
            instance.DateCreated = DateTime.UtcNow;
            instance.DateUpdated = DateTime.UtcNow;
            _dbContext.Instances.Add(instance);
            await _dbContext.SaveChangesAsync();
            return instance;
        }

        public async Task<List<Instance>> RetrieveInstances()
        {
            return await _dbContext.Instances.ToListAsync();
        }

        public async Task<Instance> RetrieveInstance(long id)
        {
            return await _dbContext.Instances.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Instance> RetrieveInstanceAsync(long id)
        {
            return await _dbContext.Instances.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> EnableInstanceAsync(long id)
        {
            var instance = await RetrieveInstanceAsync(id);
            instance.IsEnabled = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableInstanceAsync(long id)
        {
            var instance = await RetrieveInstanceAsync(id);
            instance.IsEnabled = false;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteInstanceRegistration(long id)
        {
            var instance = await RetrieveInstanceAsync(id);
            _dbContext.Instances.Remove(instance);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Default Roles and Users
        //
        // TODO - fix this after getting that async learn on!
        //
        public async Task<bool> EnsureRolesAndAdminExistAsync()
        {
            if (!await _dbContext.Roles.AnyAsync(x => x.Name == SecurityConfig.AdminRole))
            {
                _mimeoLogger.Info($"Role {SecurityConfig.AdminRole} does not exist - adding to Roles");
                var adminRole = new IdentityRole(SecurityConfig.AdminRole);
                adminRole.NormalizedName = SecurityConfig.AdminRole;
                _dbContext.Roles.Add(adminRole);
                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.Roles.AnyAsync(x => x.Name == SecurityConfig.UserRole))
            {
                _mimeoLogger.Info($"Role {SecurityConfig.UserRole} does not exist - adding to Roles");
                var userRole = new IdentityRole(SecurityConfig.UserRole);
                userRole.NormalizedName = SecurityConfig.UserRole;
                _dbContext.Roles.Add(userRole);
                await _dbContext.SaveChangesAsync();
            }

            var adminUser = await _userManager.FindByNameAsync(SecurityConfig.DefaultAdminEmail);
            if (adminUser == null)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    _mimeoLogger.Info(
                        $"Unable to locate default Sys Admin: {SecurityConfig.DefaultAdminEmail} - "
                        + @"creating new Sys Admin");

                    var newAdminUser = new ApplicationUser()
                    {
                        UserName = SecurityConfig.DefaultAdminEmail,
                        Email = SecurityConfig.DefaultAdminEmail,
                    };

                    var result = await _userManager.CreateAsync(newAdminUser, SecurityConfig.DefaultAdminPassword);
                    if (result.Succeeded == false)
                    {
                        throw new Exception($"UserManager.Create failed: {result.ToFriendlyString()}");
                    }

                    var resultAddToAdmin = await _userManager.AddToRoleAsync(newAdminUser, SecurityConfig.AdminRole);
                    if (resultAddToAdmin.Succeeded == false)
                    {
                        throw new Exception(
                            $"UserManager.AddToRole (Admin) failed: {resultAddToAdmin.ToFriendlyString()}");
                    }

                    await transaction.CommitAsync();
                }
            }

            return true;
        }


        // User management
        //
        public async Task<ApplicationUser> FindUserAsync(string emailAddress)
        {
            return await
                _dbContext
                    .Users
                    .Include(x => x.Instance)
                    .FirstOrDefaultAsync(x => x.NormalizedEmail == emailAddress);
        }

        public async Task<ApplicationUser> RetrieveUserAsync(string id)
        {
            return await _dbContext
                .Users
                .Include(x => x.Instance)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<ApplicationUser>> RetrieveAllUsersAsync()
        {
            return await _dbContext
                    .Users
                    .Include(x => x.Instance)
                    .ToListAsync();
        }

        public async Task<IList<ApplicationUser>> RetrieveUsersByInstanceAsync(long instanceId)
        { 
            return await _dbContext
                    .Users
                    .Where(x => x.Instance.Id == instanceId)
                    .ToListAsync();
        }

        public async Task<ApplicationUser> ProvisionNewAccount(string emailAddress)
        {
            var user = new ApplicationUser()
            {
                Email = emailAddress,
                UserName = emailAddress,
            };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var userFind = _userManager.FindByEmailAsync(emailAddress).Result;
                if (userFind != null)
                {
                    _mimeoLogger.Error($"User {user.Email}/{user.UserName} exists already - aborting");
                    return null;
                }

                var createUserResult = await _userManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                {
                    _mimeoLogger.Error(
                        $"Unable to create new User for {user.Email}/{user.UserName} - " +
                        $"{createUserResult.ToFriendlyString()}");
                    return null;
                }

                _mimeoLogger.Info($"Created new User {user.Email}");

                var addToRoleResult
                    = await _userManager.AddToRoleAsync(user, SecurityConfig.UserRole);
                if (!addToRoleResult.Succeeded)
                {
                    _mimeoLogger.Error(
                        $"Unable to add User {user.Email}/{user.UserName} to {SecurityConfig.UserRole} - " +
                        $"{createUserResult.ToFriendlyString()}");
                    return null;
                }

                _mimeoLogger.Info($"Added User {user.Email} to Role {SecurityConfig.UserRole}");

                transaction.Commit();
            }

            return user;
        }

        public async Task<bool> SetUserPasswordWithoutToken(ApplicationUser user, string password)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, code, password);
            return true;
        }
        public async Task<bool> UnlockUser(ApplicationUser user)
        {
            var result = await _userManager.ResetAccessFailedCountAsync(user);
            return true;
        }

        public async Task<bool> AssignUserToInstanceAsync(ApplicationUser user, Instance instance)
        {
            user.Instance = instance;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromInstance(ApplicationUser user)
        {
            user.Instance = null;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EnableUserAsync(string id)
        {
            var user = await RetrieveUserAsync(id);
            user.IsEnabled = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableUserAsync(string id)
        {
            var user = await RetrieveUserAsync(id);
            user.IsEnabled = false;
            await _dbContext.SaveChangesAsync();
            return true;
        }


        // User Claim management
        //
        public IEnumerable<Claim> RetrieveAllPossibleRoleClaims()
        {
            return new List<Claim>
            {
                new Claim(SecurityConfig.RoleClaim, SecurityConfig.UserRole),
                new Claim(SecurityConfig.RoleClaim, SecurityConfig.AdminRole),
            };
        }

        public async Task<IList<Claim>> RetrieveUserClaims(ApplicationUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }
        
        public async Task<bool> DoesUserHaveRoleClaimPersisted(IdentityUser user, string roleClaimValue)
        {
            return
                await _dbContext
                      .UserClaims
                      .AnyAsync(x => x.ClaimType == SecurityConfig.RoleClaim && x.ClaimValue == roleClaimValue);
        }

        public async Task<bool> GrantRoleClaimToUser(ApplicationUser user, string roleClaimValue)
        {
            if (await DoesUserHaveRoleClaimPersisted(user, roleClaimValue))
            {
                return false;
            }

            var claim = new Claim(SecurityConfig.RoleClaim, roleClaimValue);

            var result = await _userManager.AddClaimAsync(user, claim);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new Exception(result.ToFriendlyString());
            }
        }

        public async Task<bool> RevokeRoleClaimFromUser(ApplicationUser user, string roleClaimValue)
        {
            var claim =
                await _dbContext
                    .UserClaims
                    .FirstOrDefaultAsync(x => x.ClaimType == SecurityConfig.RoleClaim && x.ClaimValue == roleClaimValue);

            if (claim == null)
            {
                return false;
            }

            _dbContext.UserClaims.Remove(claim);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

