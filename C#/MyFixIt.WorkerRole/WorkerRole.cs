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
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.WindowsAzure.ServiceRuntime;
using MyFixIt.Common;

namespace MyFixIt.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private IContainer container;
        private ILogger logger;
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public override void Run()
        {
            logger.Information("MyFixIt.WorkerRole entry point called");

            Task task = RunAsync(tokenSource.Token);
            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unhandled exception in FixIt worker role.");
            }
        }

        private async Task RunAsync(CancellationToken token)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                IFixItQueueManager queueManager = scope.Resolve<IFixItQueueManager>();
                try
                {
                    await queueManager.ProcessMessagesAsync(token);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception in worker role Run loop.");
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            var assemblies = new[]
                             {
                                 typeof(Logging.Composition.CompositionModule).Assembly,
                                 typeof(Persistence.Composition.CompositionModule).Assembly
                             };

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(assemblies);

            container = builder.Build();

            logger = container.Resolve<ILogger>();

            return base.OnStart();
        }

        public override void OnStop()
        {
            tokenSource.Cancel();
            tokenSource.Token.WaitHandle.WaitOne();
            base.OnStop();
        }
    }
}