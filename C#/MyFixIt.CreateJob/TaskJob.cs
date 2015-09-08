using System.IO;
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

        public async Task ProcessQueueMessage([QueueTrigger("fixits")] FixItTaskMessage message, TextWriter log)
        {
            CorrelationManager.SetOperationId(message.OperationId);

            await repository.CreateAsync(message.Task);

            log.WriteLine("Created task {0}", message.Task.Title);
        }
    }
}
