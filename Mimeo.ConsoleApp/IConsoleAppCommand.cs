using Mimeo.Blocks.Helpers;

namespace Mimeo.ConsoleApp
{
    public interface IConsoleAppCommand
    {
        int Id { get; }
        void Run();
    }
    
    public static class CommandExtensions
    {
        public static void Render(this IList<IConsoleAppCommand> input)
        {
            Console.WriteLine(input.Select(x => x.ToString()).JoinByNewline());
        }
    }
}
