using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using MyFixIt.Common.Models;

namespace MyFixIt.Services
{
    internal class LoggingTaskService : ITaskService
    {
        private readonly ITaskService service;
        private readonly TelemetryClient telemetryClient = new TelemetryClient();

        public LoggingTaskService(ITaskService service)
        {
            this.service = service;
        }

        public async Task<List<FixItTask>> ListByCreatorAsync(string creator)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                return await service.ListByCreatorAsync(creator);
            }
            finally
            {
                stopWatch.Stop();
                TrackEvent("ListByCreator", stopWatch.Elapsed);
            }
        }

        public async Task CreateAsync(FixItTask task, HttpPostedFileBase photo)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                await service.CreateAsync(task, photo);
            }
            finally
            {
                stopWatch.Stop();

                var properties = new Dictionary<string, string>
                                 {
                                     { "Title", task.Title },
                                     { "Owner", task.Owner }
                                 };
                TrackEvent("Create", stopWatch.Elapsed, properties);
            }
        }

        private void TrackEvent(string eventName, TimeSpan elapsed, IDictionary<string, string> properties = null)
        {
            var telemetry = new EventTelemetry(eventName);
            telemetry.Metrics.Add("Elapsed", elapsed.TotalMilliseconds);
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    telemetry.Properties.Add(property.Key, property.Value);
                }
            }

            telemetryClient.TrackEvent(telemetry);
        }
    }
}