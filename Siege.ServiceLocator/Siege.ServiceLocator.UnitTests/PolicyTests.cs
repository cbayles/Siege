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
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Rhino.Mocks;
using Siege.ServiceLocator.Exceptions;
using Siege.ServiceLocator.RegistrationPolicies;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.UnitTests.TestClasses;
using Siege.ServiceLocator.Web.RegistrationPolicies;
using StructureMap;

namespace Siege.ServiceLocator.UnitTests
{
    [TestFixture]
    public abstract partial class ServiceLocatorTests
    {
        [Test]
        public void RegisteringTransientShouldReturnNewItem()
        {
            locator.Register<Transient>(Given<ITestInterface>.Then<TestCase1>());

            var firstInstance = locator.GetInstance<ITestInterface>();
            var secondInstance = locator.GetInstance<ITestInterface>();

            Assert.AreNotSame(firstInstance, secondInstance);
        }

        [Test]
        public void RegisteringSingletonShouldNotReturnNewItem()
        {
            locator.Register<Singleton>(Given<ITestInterface>.Then<TestCase1>());

            var firstInstance = locator.GetInstance<ITestInterface>();
            var secondInstance = locator.GetInstance<ITestInterface>();

            Assert.AreSame(firstInstance, secondInstance);
        }

        [Test]
        public void RegisteringHttpRequestShouldStoreInHttpContextItems()
        {
            var items = new Hashtable();

            var httpContextBase = MockRepository.GenerateMock<HttpContextBase>();
            httpContextBase.Stub(x => x.Items)
                .Return(items);

            locator.Register(Given<PerHttpRequest>.Then<PerHttpRequest>());
            locator.Register(Given<HttpContextBase>.ConstructWith(x => httpContextBase));

            locator.Register<PerHttpRequest>(Given<ITestInterface>.Then<TestCase1>());

            var returned = locator.GetInstance<ITestInterface>();

            foreach (DictionaryEntry item in items)
            {
                Assert.AreSame(returned,item.Value);
            }
        }


        [Test]
        public void RegisteringHttpRequestShouldReturnItemInHttpContextItems()
        {
            var items = new Hashtable();

            var httpContextBase = MockRepository.GenerateMock<HttpContextBase>();
            httpContextBase.Stub(x => x.Items)
                .Return(items);

            locator.Register(Given<PerHttpRequest>.Then<PerHttpRequest>());
            locator.Register(Given<HttpContextBase>.ConstructWith(x => httpContextBase));

            locator.Register<PerHttpRequest>(Given<ITestInterface>.Then<TestCase1>());

            var returnedFirst = locator.GetInstance<ITestInterface>();
            var returnedSecond = locator.GetInstance<ITestInterface>();

            Assert.AreSame(returnedFirst, returnedSecond);
        }

        [Test]
        public void RegisteringHttpRequestShouldNotAddedToHttpContexItemsOnSecondRequest()
        {
            var items = new Hashtable();

            var httpContextBase = MockRepository.GenerateMock<HttpContextBase>();
            httpContextBase.Stub(x => x.Items)
                .Return(items);

            locator.Register(Given<PerHttpRequest>.Then<PerHttpRequest>());
            locator.Register(Given<HttpContextBase>.ConstructWith(x => httpContextBase));

            locator.Register<PerHttpRequest>(Given<ITestInterface>.Then<TestCase1>());

            locator.GetInstance<ITestInterface>();
            locator.GetInstance<ITestInterface>();

            Assert.AreEqual(1,items.Count);
        }
    }
}