using Autofac;
using MyFixIt.Common;

namespace MyFixIt.Persistence.Composition
{
    public class CompositionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FixItQueueManager>().As<IFixItQueueManager>();
            builder.RegisterType<FixItTaskRepository>().As<IFixItTaskRepository>();
            builder.RegisterType<PhotoService>().As<IPhotoService>();
        }
    }
}