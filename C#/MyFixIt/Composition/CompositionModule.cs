using Autofac;
using Autofac.Integration.Mvc;

namespace MyFixIt.Composition
{
    public class CompositionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(CompositionModule).Assembly);
        }
    }
}