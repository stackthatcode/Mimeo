using System.Security;
using Mimeo.Blocks.Security;
using Mimeo.Communications.Config;
using Mimeo.ConsoleApp.TestWorkers;

namespace Mimeo.ConsoleApp
{
    public class ConsoleTaskService
    {
        private readonly SampleWorker16 _worker;
        private readonly MailgunConfigKeyService _configKeyService;


        public ConsoleTaskService(SampleWorker16 worker, MailgunConfigKeyService configKeyService)
        {
            this._worker = worker;
            _configKeyService = configKeyService;
        }

        public void SendEmail()
        {
            _worker.TestEmailRun();
        }

        public void SetMailGunApiKey()
        {
            Console.WriteLine("Please enter your Mailgun API key");
            var apiKey = SolicitPassword();
            _configKeyService.SetApiKey(apiKey);
        }

        public void GetMailGunApiKey()
        {
            var key = _configKeyService.GetApiKey();
            Console.WriteLine(key.ToInsecureString());
            Console.ReadKey();
            Console.Clear();
        }


        // Indeed, we can refactor as needed.
        //
        public static SecureString SolicitPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return pass.ToSecureString();
        }
    }
}

