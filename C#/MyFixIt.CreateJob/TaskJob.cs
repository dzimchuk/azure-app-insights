using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using MyFixIt.Common;
using MyFixIt.Common.Models;
using Newtonsoft.Json;

namespace MyFixIt.CreateJob
{
    internal class TaskJob
    {
        private readonly IFixItTaskRepository repository;

        public TaskJob(IFixItTaskRepository repository)
        {
            this.repository = repository;
        }

        public async Task ProcessQueueMessage([QueueTrigger("fixits")] string message, TextWriter log)
        {
            var fixit = JsonConvert.DeserializeObject<FixItTask>(message);
            await repository.CreateAsync(fixit);

            log.WriteLine(message);
        }
    }
}
