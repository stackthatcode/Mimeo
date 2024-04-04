using System;
using System.Collections.Generic;
using System.Linq;
using Mimeo.Blocks.Helpers;

namespace Mimeo.ConsoleApp
{
    public class Command<T>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Action<T> Action { get; set; }
        public const int EmptyId = -1;

        public Command(int id, string description, Action<T> action)
        {
            Id = id;
            Description = description;
            Action = action;
        }

        public Command()
        {
            Id = EmptyId;
        }

        public static Command<T> Blank()
        {
            return new Command<T>();
        }

        public override string ToString()
        {
            return Id == EmptyId ? "": $"{Id} - {Description}";
        }
    }

    public static class CommandExtensions
    {
        public static void Render<T>(this IList<Command<T>> input)
        {
            Console.WriteLine(input.Select(x => x.ToString()).JoinByNewline());
        }
    }
}


