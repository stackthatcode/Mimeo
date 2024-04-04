using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Mimeo.Middle.Identity
{
    public static class MimeoIdentityExtensions
    {
        public static IServiceCollection AddMimeoIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminsOnly", policy =>
                    policy.Requirements.Add(new RoleClaimRequiredPolicy(SecurityConfig.AdminRole)));

                options.AddPolicy("UsersOnly", policy =>
                    policy.Requirements.Add(new RoleClaimRequiredPolicy(SecurityConfig.UserRole)));
            });

            services.AddSingleton<IAuthorizationHandler, RoleClaimRequiredHandler>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                //
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Error/Dispatch/403"; // TODO - Forbidden or Unauthorized, eh?
                options.SlidingExpiration = true;
            });

            return services.Configure<IdentityOptions>(options => options.AddIdentityOptions());
        }


        public static IdentityOptions AddIdentityOptions(this IdentityOptions options)
        {
            // Password settings
            //
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings
            //
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            //
            options.User.AllowedUserNameCharacters
                = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
            options.SignIn.RequireConfirmedEmail = false;

            return options;
        }

    }
}

