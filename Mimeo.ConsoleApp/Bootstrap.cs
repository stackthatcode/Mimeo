using Autofac;
using Mimeo.Blocks;
using Mimeo.Communications;
using Mimeo.ConsoleApp.TestWorkers;
//using Mimeo.Communications;


namespace Mimeo.ConsoleApp
{
    public class Bootstrap
    {
        public Bootstrap()
        {
        }

        public IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            BlocksAutofac.Build(builder);
            CommunicationsAutofac.Build(builder);

            // Register your stuff here
            //
            builder.RegisterType<ConsoleTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<SampleWorker16>().InstancePerLifetimeScope();
            return builder.Build();
        }
    }
}
