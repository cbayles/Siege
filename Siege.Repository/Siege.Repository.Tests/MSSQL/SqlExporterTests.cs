//using System;
//using System.IO;
//using NUnit.Framework;
//using Siege.Repository.MSSQL;
//using Siege.Repository.Mapping;
//using Siege.Repository.Tests.MappingTests;

//namespace Siege.Repository.Tests.MSSQL
//{
//    [TestFixture]
//    public class SqlExporterTests
//    {
//        private DomainMap domainMap;

//        [SetUp]
//        public void SetUp()
//        {
//            domainMap = new DomainMap();
//            domainMap.Create(mapper =>
//            {
//                mapper.UseConvention(convention =>
//                {
//                    convention.WithSchema("siege");
//                    convention.WithSuffix("s");

//                    convention.IdentifyEntitiesWith<EntityIdentifier>();
//                    convention.IdentifyComponentsWith<ComponentIdentifier>();
//                    convention.IdentifyIDsWith<IdIdentifier>();

//                    convention.ForComponents(component =>
//                    {
//                        component.PrefixWith((parentType, objectType, propertyName) => "");
//                        component.SuffixWith((parentType, objectType, propertyName) => objectType.Name);
//                    });

//                    convention.ForPrimaryKeys(key => key.FormatAs(type => type.Name + "ID"));

//                    convention.ForForeignKeys(key =>
//                    {
//                        key.FormatAs(property =>
//                                    property.PropertyType.IsGenericType ?
//                                    property.DeclaringType.Name + "ID" :
//                                    property.PropertyType.Name + "ID");
//                    });
//                });

//                mapper.Add<Product>();
//                mapper.Add<OrderItem>();
//                mapper.Add<Order>();
//                mapper.Add<Customer>();
//            });
//        }

//        [Test]
//        public void ShouldOutputSql()
//        {
//            var exporter = new SqlExporter();
//            var actual = exporter.Export(domainMap);
//            var expected = File.ReadAllText("MSSQL\\SqlOutput.txt");

//            Console.WriteLine(actual);

//            Assert.AreEqual(expected, actual);
//        }
//    }
//}
