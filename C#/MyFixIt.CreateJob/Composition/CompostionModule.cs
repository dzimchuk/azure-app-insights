using Autofac;

namespace MyFixIt.CreateJob.Composition
{
    public class CompostionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TaskJob>().AsSelf();
        }
    }
}