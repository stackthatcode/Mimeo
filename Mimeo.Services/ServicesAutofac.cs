using Autofac;
using Mimeo.Services.File;

namespace Mimeo.Services
{
    public class ServicesAutofac
    {
        public static void Build(ContainerBuilder builder) //, IConfiguration configuration
        {
            builder
                .Register(x => new MockFileService(@"C:\DEV\Mimeo\TestFileStorage\"))
                .InstancePerLifetimeScope();
        }
    }
}

