﻿/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations.Named;
using Siege.ServiceLocator.Registrations.Stores;

namespace Siege.ServiceLocator.Resolution.Pipeline
{
    public class DefaultPipelineItem : BasePipelineItem<DefaultRegistrationStore>
    {
        private readonly PostResolutionPipeline pipeline;
        private readonly DefaultRegistrationStore registrationStore;

        public DefaultPipelineItem(Foundation foundation, IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store, PostResolutionPipeline pipeline) : base(foundation, serviceLocator, store)
        {
            this.pipeline = pipeline;
            this.registrationStore = foundation.StoreFor<DefaultRegistrationStore>();
        }

        protected override Func<PipelineResult> Invoker(Type type, PipelineResult value)
        {
            return () =>
            {
                if (value != null && value.Result != null) return value;

                var result = new PipelineResult();

                var registrations = registrationStore.GetRegistrationsForType(type);

                if (registrations == null) return null;

                var mappedToType = registrations[0].GetMappedToType();

                result.Name = registrations[0] is INamedRegistration
                                  ? ((INamedRegistration)registrations[0]).Key
                                  : null;
                result.MappedTo = mappedToType;
                result.MappedFrom = registrations[0].GetMappedFromType();
                result.Result = Resolve(registrations[0]);

                if(result.Result != null) result = pipeline.Execute(result);

                return result;
            };
        }
    }
}