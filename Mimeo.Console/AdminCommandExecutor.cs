using System;
using System.Linq;
using Autofac;
using Mimeo.Blocks.Helpers;
using Mimeo.Blocks.Logging;
using Mimeo.Middle.Email.Content;
using Mimeo.Middle.Email.Html;
using Mimeo.Middle.Email.Sending;
using Mimeo.Middle.Identity;
using Mimeo.Middle.Instance;

namespace Mimeo.ConsoleApp
{
    public class AdminCommandExecutor
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IdentityService _identityService;
        private readonly LoggedConsoleInput _consoleInput;
        private readonly InstanceContext _instanceContext;
        private readonly HtmlTemplateService _templateService;
        private readonly IEmailService _emailService;
        private readonly MessageBuilder _messageBuilder;
        private readonly MimeoLogger _logger;

        public AdminCommandExecutor(
                ILifetimeScope lifetimeScope,
                IdentityService identityService, 
                LoggedConsoleInput consoleInput, 
                InstanceContext instanceContext,
                HtmlTemplateService templateService,
                IEmailService emailService,
                MessageBuilder messageBuilder,
                MimeoLogger logger)
        {
            _identityService = identityService;
            _consoleInput = consoleInput;
            _instanceContext = instanceContext;
            _templateService = templateService;
            _emailService = emailService;
            _messageBuilder = messageBuilder;
            _logger = logger;
            _lifetimeScope = lifetimeScope;
        }


        #region User Account management

        public void EnsureRolesAndAdminExist()
        {
            var result = _identityService.EnsureRolesAndAdminExistAsync().Result;
        }

        public void CreateNewUserAccount()
        {
            var email = _consoleInput.SolicitString("Enter email address for new user:");
            if (!_consoleInput.ConfirmWithYes(
                    $"Are you sure you want to create a new account for {email}"))
            {
                return;
            }

            var user = _identityService.ProvisionNewAccount(email).Result;
        }

        public void SetUserPassword()
        {
            var user = FindUserByEmail();
            if (user == null)
            {
                return;
            }

            var password = _consoleInput.SolicitString("Please enter password for user:");

            var result = _identityService.SetUserPasswordWithoutToken(user, password).Result;
        }

        public void ListAllUserAccounts()
        {
            var users = _identityService.RetrieveAllUsersAsync().Result;

            foreach (var user in users)
            {
                DisplayUserInfo(user);
            }
        }

        public void AssignUserToRole()
        {
            var email = _consoleInput.SolicitUserEmail();
            if (email.IsNullOrEmpty())
            {
                return;
            }

            var user = _identityService.FindUserAsync(email).Result;
            if (user == null)
            {
                _logger.Info($"Unable to locate User Account: {email}");
                return;
            }

            var roles = _identityService.RetrieveAllPossibleRoleClaims();
            _logger.Info($"Available Roles Claims: {roles.Select(x => x.Value).ToCommaDelimited()}");

            var role = _consoleInput.SolicitString("Please enter a Role Claim:");
            if (role.IsNullOrEmpty())
            {
                return;
            }

            if (!_consoleInput.ConfirmWithYes($"Are you sure you want to assign {email} to {role}?"))
            {
                return;
            }

            _consoleInput.Processing();
            var result = _identityService.GrantRoleClaimToUser(user, role).Result;
            DisplayUserInfo(user);
        }

        public ApplicationUser FindUserByEmail()
        {
            var email = _consoleInput.SolicitUserEmail();
            if (email.IsNullOrEmpty())
            {
                return null;
            }

            var user = _identityService.FindUserAsync(email).Result;
            if (user == null)
            {
                _logger.Info($"Unable to locate User Account: {email}");
                return null;
            }
            return user;
        }

        public void RevokeUserFromRole()
        { 
            var user = FindUserByEmail();
            if (user == null)
            {
                return;
            }

            var roles = _identityService.RetrieveUserClaims(user).Result;
            _logger.Info($"Granted Roles Claims: {roles.Select(x => x.Value).ToCommaDelimited()}");

            var role = _consoleInput.SolicitString("Please enter a Role:");
            if (role.IsNullOrEmpty())
            {
                return;
            }

            if (!_consoleInput.ConfirmWithYes(
                $"Are you sure you want to revoke Role Claim {role} from {user.Email}?"))
            {
                return;
            }

            _consoleInput.Processing();
            var result = _identityService.RevokeRoleClaimFromUser(user, role).Result;
            DisplayUserInfo(user);
        }

        public void EnableUser()
        { 
            var user = FindUserByEmail();
            if (user == null)
            {
                return;
            }

            _consoleInput.Processing();
            var result = _identityService.EnableUserAsync(user.Id).Result;
            DisplayUserInfo(user);
        }

        public void DisableUser()
        { 
            var user = FindUserByEmail();
            if (user == null)
            {
                return;
            }

            _consoleInput.Processing();
            var result = _identityService.EnableUserAsync(user.Id).Result;
            DisplayUserInfo(user);
        }

        public void UnlockUser()
        {
            var user = FindUserByEmail();
            if (user == null)
            {
                return;
            }
            _consoleInput.Processing();

            var result = _identityService.UnlockUser(user).Result;
            DisplayUserInfo(user);
        }

        #endregion

        #region Instance management
        public void ListAllInstances()
        {
            var instances = _identityService.RetrieveInstances().Result;

            foreach (var instance in instances)
            {
                DisplayInstanceInfo(instance);
            }
        }

        public void CreateNewInstanceRegistration()
        {
            var instanceName = _consoleInput.SolicitString("Please enter Instance Name (required):");
            if (instanceName.IsNullOrEmpty())
            {
                return;
            }

            var databaseName = _consoleInput.SolicitString("Please enter Database Name (required):");
            if (databaseName.IsNullOrEmpty())
            {
                return;
            }

            _consoleInput.Processing();
            var instance = _identityService.CreateInstanceAsync(instanceName, databaseName, true).Result;
            DisplayInstanceInfo(instance);
        }

        public void CreateInstanceDatabase()
        {
            var instanceId = _consoleInput.SolicitInstanceId();
            if (!instanceId.HasValue)
            {
                return;
            }

            _consoleInput.Processing();

            var result = _instanceContext.InitializeAsync(instanceId.Value).Result;
            _instanceContext.InstanceDbContext.Database.EnsureCreated();
        }


        public void EnableInstance()
        {
            var instanceId = _consoleInput.SolicitInstanceId();
            if (!instanceId.HasValue)
            {
                return;
            }

            var result = _identityService.EnableInstanceAsync(instanceId.Value).Result;
            var instance = _identityService.RetrieveInstance(instanceId.Value).Result;
            DisplayInstanceInfo(instance);
        }

        public void DisableInstance()
        {
            var instanceId = _consoleInput.SolicitInstanceId();
            if (!instanceId.HasValue)
            {
                return;
            }

            var result = _identityService.DisableInstanceAsync(instanceId.Value).Result;
            var instance = _identityService.RetrieveInstance(instanceId.Value).Result;
            DisplayInstanceInfo(instance);
        }

        public void DeleteInstance()
        {
            var instanceId = _consoleInput.SolicitInstanceId();
            if (!instanceId.HasValue)
            {
                return;
            }

            var instance = _identityService.RetrieveInstance(instanceId.Value).Result;
            DisplayInstanceInfo(instance);

            if (!_consoleInput.ConfirmWithYes(
                $"Are you ABSOLUTELY sure you want to delete the Instance Registration for Instance {instance.FriendlyId}?")
            )
            {
                return;
            }

            _consoleInput.Processing();
            var result = _identityService.DeleteInstanceRegistration(instanceId.Value).Result;
        }

        public void ListUsersByInstance(long? instanceId = null)
        {
            if (instanceId == null)
            {
                instanceId = _consoleInput.SolicitInstanceId();
                if (!instanceId.HasValue)
                {
                    return;
                }
            }

            var instance = _identityService.RetrieveInstanceAsync(instanceId.Value).Result;
            var users = _identityService.RetrieveUsersByInstanceAsync(instanceId.Value).Result;

            foreach (var user in users)
            {
                DisplayUserInfo(user);
            }
        }

        public void AssignUserToInstance()
        {
            var user = FindUserByEmail();
            if (user == null)
            {
                return;
            }
            DisplayUserInfo(user);

            var instanceId = _consoleInput.SolicitInstanceId();
            if (instanceId == null)
            {
                return;
            }

            var instance = _identityService.RetrieveInstance(instanceId.Value).Result;
            DisplayInstanceInfo(instance);

            if (!_consoleInput.ConfirmWithYes($"Are you sure you want to assign User {user} to Instance {instance}?"))
            {
                return;
            }

            var result = _identityService.AssignUserToInstanceAsync(user, instance).Result;
            ListUsersByInstance(instanceId.Value);
        }

        public void RemoveUserFromInstance()
        {
            var user = FindUserByEmail();
            if (user == null)
            {
                return;
            }
            DisplayUserInfo(user);

            var instance = _identityService.RetrieveInstance(user.InstanceId.Value).Result;

            if (!_consoleInput.ConfirmWithYes(
                    $"Are you sure you want to remove User {user} from Instance {instance}?"))
            {
                return;
            }

            var result = _identityService.RemoveUserFromInstance(user).Result;
        }
        #endregion

        private void DisplayUserInfo(ApplicationUser user)
        {
            _logger.Info($"User: {user.Email} (Id: {user.Id})");

            // TODO - add navigation properties to EF
            //
            var userRoles = _identityService.RetrieveUserClaims(user).Result;

            _logger.Info(userRoles.Count > 0
                ? $"Roles Claims: {userRoles.Select(x => x.Value).ToCommaDelimited()}"
                : "Roles Claims: (none)");

            _logger.Info(user.Instance != null
                ? $"Instance: {user.Instance.ToString()}"
                : "Instance: (none)");

            _logger.Info($"Account Enabled: {user.IsEnabled.ToYesNo()}");

            _logger.Info("");
        }

        private void DisplayInstanceInfo(Instance input)
        {

            _logger.Info(
                $"Instance Id: {input.FriendlyId} - " +
                $"Name: {input.InstanceName} - " +
                $"Database: {input.Database} - " + 
                $"IsEnabled: {input.IsEnabled.ToYesNo()}" + 
                Environment.NewLine);
        }


        #region Test functions
        public void SendTestEmail()
        {
            var message
                = _messageBuilder
                    .AddLogo()
                    .AddHtmlTextBlock("Hey, this is only a test! Hey, this is only a test! Hey, this is only a test! Hey, this is only a test! Hey, this is only a test!")
                    .AddActionButton("http://google.com", "Click this!")
                    .Output();

            var htmlMessage 
                = _templateService
                    .Initialize()
                    .Build(message, false);

            _emailService.Send("aleksjones@gmail.com", "Test Email!", htmlMessage);
        }
        #endregion

        public void RunHangfireBackgroundJob()
        {


        }
    }
}

