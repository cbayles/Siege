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
using System.Collections.Generic;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations;
using Siege.ServiceLocator.Registrations.Named;
using Siege.ServiceLocator.Registrations.Stores;

namespace Siege.ServiceLocator.Resolution.Pipeline
{
    public class ConditionalPipelineItem : BasePipelineItem<ConditionalRegistrationStore>
    {
        private readonly PostResolutionPipeline pipeline;
        private readonly ConditionalRegistrationStore registrationStore;
        public ConditionalPipelineItem(Foundation foundation, IServiceLocatorAdapter serviceLocator, IServiceLocatorStore store, PostResolutionPipeline pipeline) : base(foundation, serviceLocator, store)
        {
            this.pipeline = pipeline;
            this.registrationStore = foundation.StoreFor<ConditionalRegistrationStore>();
        }

        protected override Func<PipelineResult> Invoker(Type type, PipelineResult value)
        {
            return () =>
            {
                if (value != null && value.Result != null) return value;

                var result = new PipelineResult();
                IList<IRegistration> conditionalCases = registrationStore.GetRegistrationsForType(type);

                if (conditionalCases == null) return null;

                for (int i = 0; i < conditionalCases.Count; i++)
                {
                    var registration = conditionalCases[i];
                    
                    if (registration.IsValid(serviceLocator, store))
                    {
                        var mappedToType = registration.GetMappedToType();

                        result.Name = registration is INamedRegistration
                                          ? ((INamedRegistration) registration).Key
                                          : null;
                        result.MappedTo = mappedToType;
                        result.MappedFrom = registration.GetMappedFromType();
                        result.Result = Resolve(registration);
                        
                        if(result.Result != null) result = pipeline.Execute(result);
                        return result;
                    }
                }

                return null;
            };
        }
    }
}