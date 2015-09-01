using System;
using System.Linq;
using Autofac;
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
    }
}
