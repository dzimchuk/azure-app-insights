using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using MyFixIt.Common;
using MyFixIt.Common.Models;

namespace MyFixIt.CreateJob
{
    public class TaskJob
    {
        private readonly IFixItTaskRepository repository;

        public TaskJob(IFixItTaskRepository repository)
        {
            this.repository = repository;
        }

        public async Task ProcessQueueMessage([QueueTrigger("fixits")] FixItTask task, TextWriter log)
        {
            CallContext.LogicalSetData(CorrelatingTelemetryInitializer.OPERATION_ID, Guid.NewGuid().ToString());

            await repository.CreateAsync(task);

            log.WriteLine("Created task {0}", task.Title);
        }
    }
}
