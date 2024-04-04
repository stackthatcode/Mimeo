using Hangfire;
using Hangfire.SqlServer;


namespace Mimeo.Middle.Hangfire
{
    public class HangfireShim
    {
        public static void Configure(string connectionString)
        {
            GlobalConfiguration
                .Configuration
                .UseSqlServerStorage(
                    connectionString,
                    new SqlServerStorageOptions
                    {
                        PrepareSchemaIfNecessary = true
                    });
        }
    }
}

