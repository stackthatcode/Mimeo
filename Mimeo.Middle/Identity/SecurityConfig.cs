namespace Mimeo.Middle.Identity
{
    public class SecurityConfig
    {
        // Claim Types 
        //
        public const string RoleClaim = "ROLECLAIM";

        // Claim Values
        //
        public const string UserRole = "USER";
        public const string AdminRole = "ADMIN";


        // Security Defaults
        public const string DefaultAdminEmail = "info@logicautomated.com";
        public const string DefaultAdminPassword = "123456";
    }
}
