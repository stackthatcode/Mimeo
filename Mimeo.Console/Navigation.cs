using System;
using System.Collections.Generic;
using System.Linq;
using Mimeo.Blocks.Helpers;

namespace Mimeo.ConsoleApp
{
    public class Navigation
    {
        private static readonly int RunHangfireBackgroundCommandId = 1;
        private static readonly int AcctMgmtCommandId = 11;
        private static readonly int InstMgmtCommandId = 21;
        private static readonly int TestingCommandId = 31;

        private static readonly 
                List<Command<AdminCommandExecutor>> RootMenuCommands 
                    = new List<Command<AdminCommandExecutor>>()
        {
            new Command<AdminCommandExecutor>(RunHangfireBackgroundCommandId, "Run Hangfire Background Job", x => x.RunHangfireBackgroundJob()),
            Command<AdminCommandExecutor>.Blank(),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "Ensure that Roles and Admin User exist", x => x.EnsureRolesAndAdminExist()),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "Create new User Account", x => x.CreateNewUserAccount()),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "Set User Account Password", x => x.SetUserPassword()),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "List all User Accounts", x => x.ListAllUserAccounts()),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "Grant Role Claim to User", x => x.AssignUserToRole()),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "Revoke Role Claim from User", x => x.RevokeUserFromRole()),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "Disable User Account", x => x.DisableUser()),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "Enable User Account", x => x.EnableUser()),
            new Command<AdminCommandExecutor>(AcctMgmtCommandId++, "Unlock User Account", x => x.UnlockUser()),
            Command<AdminCommandExecutor>.Blank(),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "List all Instances", x => x.ListAllInstances()),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "Create new Instance Registration", x => x.CreateNewInstanceRegistration()),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "Delete Instance Registration", x => x.DeleteInstance()),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "Create new Instance Database", x => x.CreateInstanceDatabase()),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "Enable Instance", x => x.EnableInstance()),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "Disable Instance", x => x.DisableInstance()),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "List all Users by Instance", x => x.ListUsersByInstance()),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "Assign User to Instance", x => x.AssignUserToInstance()),
            new Command<AdminCommandExecutor>(InstMgmtCommandId++, "Remove User from Instance", x => x.RemoveUserFromInstance()),
            Command<AdminCommandExecutor>.Blank(),
            new Command<AdminCommandExecutor>(TestingCommandId++, "Send Test Email", x => x.SendTestEmail()),
            Command<AdminCommandExecutor>.Blank(),
        };

        public static void MainLoop()
        {
            RootMenuCommands.Render();

            while (MainLoopAux()) { }

            HitEnterToContinue("FIN");
        }

        public static bool MainLoopAux()
        {
            var selection = SolicitCommandChoice();
            if (selection == "H")
            {
                RootMenuCommands.Render();
                return true;
            }

            if (!selection.IsInteger())
            {
                return false;
            }

            var command = RootMenuCommands.FirstOrDefault(x => x.Id == selection.ToInteger());
            if (command == null)
            {
                Console.WriteLine($"Unable to recognize command: '{selection}'");
                return true;
            }

            if (selection.ToInteger() == RunHangfireBackgroundCommandId)
            {
                Bootstrap.RunHangfireBackgroundJob();
                return true;
            }

            Bootstrap.Execute(command.Action, command.Description);
            return true;
        }

        public static string SolicitCommandChoice()
        {
            Console.WriteLine("Enter a command and hit ENTER (Use 'H' to list commands):");
            return Console.ReadLine().Trim().ToUpper();
        }

        public static void HitEnterToContinue(string message = null)
        {
            Console.WriteLine(message ?? "Hit enter to continue...");
            Console.ReadLine();
        }
    }
}

