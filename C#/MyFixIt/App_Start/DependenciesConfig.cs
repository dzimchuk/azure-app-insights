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
using Autofac;
using System.Linq;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using MyFixIt.Logging.Composition;

namespace MyFixIt.App_Start
{
    public class DependenciesConfig
    {
        public static IDependencyResolver RegisterDependencies()
        {
            var assemblies = new[]
                             {
                                 typeof(CompositionModule).Assembly,
                                 typeof(Persistence.Composition.CompositionModule).Assembly,
                                 typeof(MvcApplication).Assembly
                             };

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(assemblies);

            var container = builder.Build();

            var dependencyResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(dependencyResolver);

            return dependencyResolver;
        }
    }
}