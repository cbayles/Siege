using System.Runtime.CompilerServices;
using System.Web;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.RegistrationPolicies;
using Siege.ServiceLocator.Resolution.Pipeline;

/*   Copyright 2009 - 2010 Marcus Bratton

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

namespace Siege.ServiceLocator.Web.RegistrationPolicies
{
    public sealed class PerHttpRequest : AbstractRegistrationPolicy
    {
        private readonly object lockObject = new object();
        private readonly string key;

        public PerHttpRequest()
        {
           key = string.Format("siege-perhttp-{0}", GetHashCode());
        }

        public override object ResolveWith(IInstanceResolver locator, IServiceLocatorStore context, PostResolutionPipeline pipeline)
        {
            var httpContext = locator.GetInstance<HttpContextBase>();
            var instance = httpContext.Items[key];
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = registration.ResolveWith(locator, context, pipeline);
                        httpContext.Items.Add(key, instance);
                        return instance;
                    }
                }
            }

            return httpContext.Items[key];
        }

    }
}
