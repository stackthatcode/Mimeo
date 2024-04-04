using Autofac;

namespace Mimeo.ConsoleApp
{
    public class BootstrapCommand<T> : IConsoleAppCommand
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Action<T> Action { get; set; }
        public const int EmptyId = -1;

        public BootstrapCommand(int id, string description, Action<T> action)
        {
            Id = id;
            Description = description;
            Action = action;
        }

        public BootstrapCommand()
        {
            Id = EmptyId;
        }

        public void Run()
        {
            var container = new Bootstrap().BuildContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                var instance = scope.Resolve<T>();
                Action(instance);
            }
        }

        public static BootstrapCommand<T> Blank()
        {
            return new BootstrapCommand<T>();
        }

        public override string ToString()
        {
            return Id == EmptyId ? "": $"{Id} - {Description}";
        }
    }
}

