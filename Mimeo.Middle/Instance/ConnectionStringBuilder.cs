namespace Mimeo.Middle.Instance
{
    public class ConnectionStringBuilder
    {
        public static string Build(string instanceDatabase)
        {
            return $"Server=localhost; Database={instanceDatabase}; Trusted_Connection=True;";
        }
    }
}