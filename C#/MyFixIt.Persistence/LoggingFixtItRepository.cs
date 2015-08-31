using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MyFixIt.Common;
using MyFixIt.Common.Models;

namespace MyFixIt.Persistence
{
    internal class LoggingFixtItRepository : IFixItTaskRepository, IDisposable
    {
        private readonly IFixItTaskRepository repository;
        private readonly ILogger logger;

        public LoggingFixtItRepository(IFixItTaskRepository repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<List<FixItTask>> FindOpenTasksByOwnerAsync(string userName)
        {
            var timespan = Stopwatch.StartNew();

            try
            {
                var result = await repository.FindOpenTasksByOwnerAsync(userName);

                timespan.Stop();
                logger.TraceApi("SQL Database", "FixItTaskRepository.FindTasksByOwnerAsync", timespan.Elapsed, "username={0}", userName);

                return result;
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in FixItTaskRepository.FindTasksByOwnerAsync(userName={0})", userName);
                throw;
            }
        }

        public async Task<List<FixItTask>> FindTasksByCreatorAsync(string creator)
        {
            var timespan = Stopwatch.StartNew();

            try
            {
                var result = await repository.FindTasksByCreatorAsync(creator);

                timespan.Stop();
                logger.TraceApi("SQL Database", "FixItTaskRepository.FindTasksByCreatorAsync", timespan.Elapsed, "creater={0}", creator);

                return result;
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in FixItTaskRepository.FindTasksByCreatorAsync(creater={0})", creator);
                throw;
            }
        }

        public async Task<FixItTask> FindTaskByIdAsync(int id)
        {
            FixItTask fixItTask;
            var timespan = Stopwatch.StartNew();

            try
            {
                fixItTask = await repository.FindTaskByIdAsync(id);

                timespan.Stop();
                logger.TraceApi("SQL Database", "FixItTaskRepository.FindTaskByIdAsync", timespan.Elapsed, "id={0}", id);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in FixItTaskRepository.FindTaskByIdAsync(id={0})", id);
                throw;
            }

            return fixItTask;
        }

        public async Task CreateAsync(FixItTask taskToAdd)
        {
            var timespan = Stopwatch.StartNew();

            try
            {
                await repository.CreateAsync(taskToAdd);

                timespan.Stop();
                logger.TraceApi("SQL Database", "FixItTaskRepository.CreateAsync", timespan.Elapsed, "taskToAdd={0}", taskToAdd);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in FixItTaskRepository.CreateAsync(taskToAdd={0})", taskToAdd);
                throw;
            }
        }

        public async Task UpdateAsync(FixItTask taskToSave)
        {
            var timespan = Stopwatch.StartNew();

            try
            {
                await repository.UpdateAsync(taskToSave);

                timespan.Stop();
                logger.TraceApi("SQL Database", "FixItTaskRepository.UpdateAsync", timespan.Elapsed, "taskToSave={0}", taskToSave);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in FixItTaskRepository.UpdateAsync(taskToSave={0})", taskToSave);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var timespan = Stopwatch.StartNew();

            try
            {
                await repository.DeleteAsync(id);

                timespan.Stop();
                logger.TraceApi("SQL Database", "FixItTaskRepository.DeleteAsync", timespan.Elapsed, "id={0}", id);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in FixItTaskRepository.DeleteAsync(id={0})", id);
                throw;
            }
        }

        public void Dispose()
        {
            var disposable = repository as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}