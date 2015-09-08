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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Siege.ServiceLocator.Registrations.AutoLoader
{
    public class Load
    {
        public static Action<IServiceLocator> FromAssembliesIn(string binPath, string scriptFolder, string fileExtension)
        {
            return locator =>
            {
                var scripts = new List<string>();
                var files = Directory.Exists(scriptFolder) ? Directory.GetFiles(scriptFolder).ToList() : new List<string>();
                var binFiles = Directory.Exists(binPath) ? Directory.GetFiles(binPath).ToList() : new List<string>();
                var loadedAssembles = AppDomain.CurrentDomain.GetAssemblies();

                if (files.Count == 0) return;

                files.ForEach(file =>
                {
                    if (file.EndsWith(fileExtension)) scripts.Add(file);
                });

                binFiles.ForEach(file =>
                {
                    if (!file.EndsWith(".dll") && !file.EndsWith(".exe")) return;
                    if (loadedAssembles.Any(x => x.FullName == Path.GetFileName(file))) return;

                    try
                    {
                        Assembly.LoadFrom(file);
                    }
                    catch (BadImageFormatException ex)
                    {
                    }
                });

                files.ForEach(file =>
                {
                    if (!file.EndsWith(".dll") && !file.EndsWith(".exe")) return;

                    try
                    {
                        var types = Assembly.LoadFrom(file).GetTypes().ToList();

                        types.ForEach(type =>
                        {
                            if (type.GetInterfaces().Contains(typeof(IAutoloader)))
                            {
                                var instance = (IAutoloader)type.GetConstructor(new Type[] { }).Invoke(new object[] { });

                                locator.Register(instance.Load(scripts));
                            }
                        });
                    }
                    catch (BadImageFormatException ex)
                    {
                    }
                });
            };
        }
    }
}