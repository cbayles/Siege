using System;
using System.Linq;
using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class ListMapping : ElementMapping
    {
        private readonly Type type;
        private readonly Type parentType;
        private ReverseForeignRelationshipMapping foreignRelationshipMapping;

        public ListMapping(PropertyInfo property, Type type, Type parentType) : base(property)
        {
            this.type = type;
            this.parentType = parentType;
        }

        public ReverseForeignRelationshipMapping ForeignRelationshipMapping
        {
            get { return foreignRelationshipMapping; }
        }

        public Type Type
        {
            get { return type; }
        }

        public override void ExportTo(IDialect exporter)
        {
            //foreignRelationshipMapping.ExportTo(exporter);
        }

        public override void Build(DomainMapper masterMap, Formatter<PropertyInfo> formatter)
        {
            base.Build(masterMap, formatter);
            var mappedType = masterMap.For(Property.PropertyType.GetGenericArguments()[0]);

            if(!mappedType.SubMappings.Any()) throw new Exception(string.Format("No mappings found type {0}", Property.PropertyType.GetGenericArguments()[0]));

            var foreignKey = mappedType.SubMappings.OfType<IdMapping>().First().Property;
            this.foreignRelationshipMapping = new ReverseForeignRelationshipMapping(Property, type, parentType, foreignKey, formatter);
            masterMap.For(type).Map(mapping => mapping.MapForeignRelationship(masterMap, Property, type, formatter));
        }
    }
}