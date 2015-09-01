using System;
using System.Configuration;
using System.Linq;
using Autofac;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;

namespace MyFixIt.CreateJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            InitializeAppInsights();

            var container = InitializeContainer();
            var config = new JobHostConfiguration { JobActivator = new AutofacJobActivator(container) };

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
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
