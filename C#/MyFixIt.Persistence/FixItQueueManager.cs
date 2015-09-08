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

using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using MyFixIt.Common;
using MyFixIt.Common.Models;
using Newtonsoft.Json;

namespace MyFixIt.Persistence
{
    internal class FixItQueueManager : IFixItQueueManager
    {
        private readonly CloudQueueClient queueClient;
        private readonly IFixItTaskRepository repository;

        private const string FixitQueueName = "fixits";

        public FixItQueueManager(IFixItTaskRepository repository)
        {
            this.repository = repository;
            CloudStorageAccount storageAccount = StorageUtils.StorageAccount;
            queueClient = storageAccount.CreateCloudQueueClient();
        }

        // Puts a serialized fixit onto the queue.
        public async Task SendMessageAsync(FixItTask fixIt)
        {
            CloudQueue queue = queueClient.GetQueueReference(FixitQueueName);
            await queue.CreateIfNotExistsAsync();

            var fixitJson = JsonConvert.SerializeObject(new FixItTaskMessage
                                                        {
                                                            Task = fixIt,
                                                            OperationId = CorrelationManager.GetOperationId()
                                                        });
            CloudQueueMessage message = new CloudQueueMessage(fixitJson);

            await queue.AddMessageAsync(message);
        }

        // Processes any messages on the queue.
        public async Task ProcessMessagesAsync(CancellationToken token)
        {
            CloudQueue queue = queueClient.GetQueueReference(FixitQueueName);
            await queue.CreateIfNotExistsAsync();

            while (!token.IsCancellationRequested)
            {
                // The default timeout is 90 seconds, so we won’t continuously poll the queue if there are no messages.
                // Pass in a cancellation token, because the operation can be long-running.
                CloudQueueMessage message = await queue.GetMessageAsync(token);
                if (message != null)
                {
                    FixItTask fixit = JsonConvert.DeserializeObject<FixItTaskMessage>(message.AsString).Task;
                    await repository.CreateAsync(fixit);
                    await queue.DeleteMessageAsync(message);
                }
            }
        }
    }
}