using System;
using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class ReverseForeignRelationshipMapping : ForeignRelationshipMapping
    {
        private readonly Type type;
        private readonly Type parentType;

        public ReverseForeignRelationshipMapping(PropertyInfo property, Type type, Type parentType, PropertyInfo foreignKey, Formatter<PropertyInfo> keyFormatter)
            : base(property, foreignKey, keyFormatter)
        {
            this.type = type;
            this.parentType = parentType;
        }
    }
}