using System.Security;
using Mimeo.Blocks.Helpers;
using Mimeo.Blocks.Security;

namespace Mimeo.ConsoleApp
{
    public class Navigation
    {
        private static readonly int CommandNumbering = 1;

        private static readonly
                List<IConsoleAppCommand> RootMenuCommands = new List<IConsoleAppCommand>()
        {
            new BootstrapCommand<ConsoleTaskService>(
                CommandNumbering++,
                "Send Email (TEST 0016)", 
                x => x.SendEmail()),

            new BootstrapCommand<ConsoleTaskService>(
                CommandNumbering++, "Set Mailgun API Key", 
                x => x.SetMailGunApiKey()),

            new BootstrapCommand<ConsoleTaskService>(
                CommandNumbering++, "Get Mailgun API Key",
                x => x.GetMailGunApiKey()),

            //GetMailGunApiKey
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
            }
            else
            {
                command.Run();
            }

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
