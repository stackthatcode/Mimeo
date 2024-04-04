using System;


namespace Mimeo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
                "Mimeo Commandline App" + Environment.NewLine +
                "+++++++++++++++++++++" + Environment.NewLine +
                "Logic Automated LLC - all rights reserved" + Environment.NewLine);


            Navigation.MainLoop();
        }
    }
}

