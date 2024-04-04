using System;
using Mimeo.Blocks.Helpers;
using Mimeo.Blocks.Logging;

namespace Mimeo.ConsoleApp
{
    public class LoggedConsoleInput
    {
        private static long TestInstanceId = 1;

        private readonly MimeoLogger _logger;

        public LoggedConsoleInput(MimeoLogger logger)
        {
            _logger = logger;
        }

        public long? SolicitInstanceId()
        {
            _logger.Info(Environment.NewLine + $"Enter Instance Id:");
            var input = Console.ReadLine();

            if (input.IsNullOrEmpty())
            {
                return null;
            }
            _logger.Info($"You entered: {input}");
            return input.ToLongNullableAlt();
        }

        public string SolicitUserEmail()
        {
            _logger.Info(Environment.NewLine + "Enter User Account email address:");
            var output = Console.ReadLine()?.Trim();
            _logger.Info($"You entered: {output}");
            return output;
        }

        public string SolicitString(string message)
        {
            Console.WriteLine(Environment.NewLine + message);
            var output = Console.ReadLine()?.Trim();
            _logger.Info($"You entered: {output}");
            return output;
        }

        public bool ConfirmWithYes(string message)
        {
            if (!message.Trim().IsNullOrEmpty())
            {
                Console.WriteLine(Environment.NewLine + message);
            }

            Console.WriteLine(Environment.NewLine + "Please type 'YES' to proceed");
            var input = Console.ReadLine();
            return input.Trim() == "YES";
        }

        public void Processing()
        {
            _logger.Info("Processing...");
        }
    }
}

