//
// Copyright (C) Microsoft Corporation.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyFixIt.Common;
using MyFixIt.Common.Models;

namespace MyFixIt.Persistence
{
    internal class FixItTaskRepository : IFixItTaskRepository, IDisposable
    {
        private MyFixItContext context;

        public FixItTaskRepository(MyFixItContext context)
        {
            this.context = context;
        }

        public Task<FixItTask> FindTaskByIdAsync(int id)
        {
            return context.FixItTasks.FindAsync(id);
        }

        public Task<List<FixItTask>> FindOpenTasksByOwnerAsync(string userName)
        {
            return context.FixItTasks
                          .Where(t => t.Owner == userName)
                          .Where(t => t.IsDone == false)
                          .OrderByDescending(t => t.FixItTaskId).ToListAsync();
        }

        public Task<List<FixItTask>> FindTasksByCreatorAsync(string creator)
        {
            return context.FixItTasks
                          .Where(t => t.CreatedBy == creator)
                          .OrderByDescending(t => t.FixItTaskId).ToListAsync();
        }

        public Task CreateAsync(FixItTask taskToAdd)
        {
            context.FixItTasks.Add(taskToAdd);
            return context.SaveChangesAsync();
        }

        public Task UpdateAsync(FixItTask taskToSave)
        {
            context.Entry(taskToSave).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Int32 id)
        {
            var fixittask = await context.FixItTasks.FindAsync(id);
            context.FixItTasks.Remove(fixittask);
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }
    }
}