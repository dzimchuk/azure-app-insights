using Autofac;
using MyFixIt.Common;

namespace MyFixIt.Logging.Composition
{
    public class CompositionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        }
    }
}