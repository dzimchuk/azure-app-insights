using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using MyFixIt.Common;
using MyFixIt.Common.Models;

namespace MyFixIt.Services
{
    internal class TaskService : ITaskService
    {
        private readonly IFixItTaskRepository fixItRepository;
        private readonly IPhotoService photoService;
        private readonly IFixItQueueManager queueManager;

        public TaskService(IFixItTaskRepository fixItRepository, IPhotoService photoService, IFixItQueueManager queueManager)
        {
            this.fixItRepository = fixItRepository;
            this.photoService = photoService;
            this.queueManager = queueManager;
        }

        public Task<List<FixItTask>> ListByCreatorAsync(string creator)
        {
            return fixItRepository.FindTasksByCreatorAsync(creator);
        }

        public async Task CreateAsync(FixItTask task, HttpPostedFileBase photo)
        {
            if (task.Notes.Contains("fail me"))
            {
                throw new Exception("Task cannot be created");
            }
            
            task.PhotoUrl = await photoService.UploadPhotoAsync(photo);

            if (ConfigurationManager.AppSettings["UseQueues"] == "true")
            {
                await queueManager.SendMessageAsync(task);
            }
            else
            {
                await fixItRepository.CreateAsync(task);
            }
        }
    }
}