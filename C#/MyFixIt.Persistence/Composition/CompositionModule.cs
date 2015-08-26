using Autofac;
using MyFixIt.Common;

namespace MyFixIt.Persistence.Composition
{
    public class CompositionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FixItQueueManager>().As<IFixItQueueManager>();
            builder.RegisterType<PhotoService>().As<IPhotoService>();

            builder.RegisterType<MyFixItContext>().AsSelf();
            builder.RegisterType<FixItTaskRepository>().Named<IFixItTaskRepository>("fixitRepo");
            builder.RegisterDecorator<IFixItTaskRepository>((c, inner) => new LoggingFixtItRepository(inner, c.Resolve<ILogger>()), fromKey: "fixitRepo");
        }
    }
}