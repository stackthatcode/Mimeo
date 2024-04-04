using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mimeo.Blocks.Logging;
using Mimeo.Middle.Config;
using Mimeo.Middle.Email.Content;
using Mimeo.Middle.Email.Html;
using Mimeo.Middle.Email.Sending;
using Mimeo.Middle.Hangfire;
using Mimeo.Middle.Identity;
using Mimeo.Middle.Instance;

namespace Mimeo.Middle
{
    public class MiddleAutofac
    {
        public static void BuildAppSettings(ContainerBuilder builder, MimeoAppSettings appSettings)
        {
            builder.Register(x => appSettings).As<MimeoAppSettings>().SingleInstance();
        }

        // Instance-specific registrations
        //
        public static void Build(ContainerBuilder builder, IConfigurationRoot configuration)
        {
            // Logging 
            // 
            builder.RegisterType<MimeoLogger>().InstancePerLifetimeScope();

            // Instance-level stuff
            //
            builder.RegisterType<InstanceContext>().InstancePerLifetimeScope();

            // Hangfire 
            //
            builder.RegisterType<JobMonitoringService>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(JobRunner<>)).InstancePerLifetimeScope();
            builder.RegisterType<JobScheduler>().InstancePerLifetimeScope();
            builder.RegisterType<RecurringJobManger>().InstancePerLifetimeScope();
            builder.RegisterType<RunLockRepository>().InstancePerLifetimeScope();

            // Email Components
            //
            builder.RegisterType<MessageBuilder>();
            builder.RegisterType<HtmlTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<TestEmailService>().As<IEmailService>().InstancePerLifetimeScope();
        }

        public static void BuildIdentityRegistrations(ContainerBuilder builder, IConfigurationRoot configurationRoot)
        {
            // Most registrations are scoped per lifetime of HTTP request and/or Hangfire Job execution
            //
            builder.RegisterType<ApplicationUser>().As<IdentityUser>();

            var options
                = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(configurationRoot.GetConnectionString("DefaultConnection"))
                    .EnableDetailedErrors(true)
                    .Options;
            builder
                .Register(ctx => new ApplicationDbContext(options))
                .As<ApplicationDbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DefaultEmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultNormalizer>().As<ILookupNormalizer>().InstancePerLifetimeScope();
            builder.RegisterType<IdentityErrorDescriber>().InstancePerLifetimeScope();

            builder
                .Register(x => new IdentityOptions().AddIdentityOptions())
                .As<IdentityOptions>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserValidator<ApplicationUser>>()
                .As<IUserValidator<ApplicationUser>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<PasswordValidator<ApplicationUser>>()
                .As<IPasswordValidator<ApplicationUser>>()
                .InstancePerLifetimeScope();
            
            builder
                .RegisterType<UserClaimsPrincipalFactory<ApplicationUser>>()
                .As<IUserClaimsPrincipalFactory<ApplicationUser>>()
                .InstancePerLifetimeScope();
           
            builder
                .RegisterType<DefaultUserConfirmation<ApplicationUser>>()
                .As<IUserConfirmation<ApplicationUser>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<PasswordHasher<ApplicationUser>>()
                .As<IPasswordHasher<ApplicationUser>>()
                .InstancePerLifetimeScope();
            
            builder
                .Register(ctx => new UserStore<ApplicationUser>(ctx.Resolve<ApplicationDbContext>()))
                .As<IUserStore<ApplicationUser>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserManager<ApplicationUser>>().InstancePerLifetimeScope();
            builder.RegisterType<SignInManager<ApplicationUser>>().InstancePerLifetimeScope();
            builder.RegisterType<RoleManager<IdentityRole>>().InstancePerLifetimeScope();


            // High-level service for interacting with Identity
            //
            builder.RegisterType<IdentityService>().InstancePerLifetimeScope();
        }
    }
}

