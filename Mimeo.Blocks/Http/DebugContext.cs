using System;

namespace Mimeo.Blocks.Http
{
    public class DebugContext
    {
        public string Verb { get; set; }
        public string Url { get; set; }
        public string Contents { get; set; }

        public DebugContext(string verb, string url, string contents = null)
        {
            Verb = verb;
            Url = url;
            Contents = contents;
        }

        public override string ToString()
        {
            return $"{Verb} - {Url}{Environment.NewLine}{Contents}";
        }
    }
}
