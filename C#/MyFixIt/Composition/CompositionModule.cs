using Autofac;
using Autofac.Integration.Mvc;
using MyFixIt.Services;

namespace MyFixIt.Composition
{
    public class CompositionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TaskService>().Named<ITaskService>("taskService");
            builder.RegisterDecorator<ITaskService>(inner => new LoggingTaskService(inner), fromKey: "taskService");

            builder.RegisterControllers(typeof(CompositionModule).Assembly);
        }
    }
}