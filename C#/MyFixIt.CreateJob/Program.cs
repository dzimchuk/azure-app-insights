using System.Configuration;
using Autofac;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using MyFixIt.Common;

namespace MyFixIt.CreateJob
{
    class Program
    {
        static void Main()
        {
            InitializeAppInsights();

            var container = InitializeContainer();
            var config = new JobHostConfiguration { JobActivator = new AutofacJobActivator(container) };

            var host = new JobHost(config);
            host.RunAndBlock();
        }

        private static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<Persistence.Composition.CompositionModule>();
            builder.RegisterModule<Logging.Composition.CompositionModule>();
            builder.RegisterModule<Composition.CompostionModule>();

            return builder.Build();
        }

        private static void InitializeAppInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["ApplicationInsights.InstrumentationKey"];
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new CorrelatingTelemetryInitializer());
        }
    }
}
