using Autofac;
using Mimeo.Blocks.Http;

namespace Mimeo.Blocks
{
    public class BlocksAutofac
    {
        public static void Build(ContainerBuilder builder)
        {
            builder.RegisterType<Throttler>().InstancePerLifetimeScope();
            builder.RegisterType<ExecutorFactory>().InstancePerLifetimeScope();
            builder.RegisterType<FaultTolerantExecutor>().InstancePerLifetimeScope();
        }
    }
}
