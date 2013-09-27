using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;
using Siege.Repository.Mapping.PropertyMappings;

namespace Siege.Repository.Mapping
{
    public class DomainMapping<TClass> : DomainMapping
    {
        public DomainMapping() : base(typeof(TClass))
        {
        }

        public DomainMapping<TClass> MapID<TType>(Expression<Func<TClass, TType>> expression)
        {
            this.subMappings.Add(new IdMapping<TClass, TType>(expression, (((MemberExpression)expression.Body).Member as PropertyInfo).Name));
            return this;
        }

        public DomainMapping<TClass> MapID<TType>(Expression<Func<TClass, TType>> expression, Formatter<Type> columnName)
        {
            this.subMappings.Add(new IdMapping<TClass, TType>(expression, columnName));
            return this;
        }

        public DomainMapping<TClass> MapID<TType>(Expression<Func<TClass, TType>> expression, string columnName)
        {
            this.subMappings.Add(new IdMapping<TClass, TType>(expression, columnName));
            return this;
        }

        public DomainMapping<TClass> ToTable(string tableName)
        {
            this.table = new Table { Name = tableName };
            return this;
        }

        public DomainMapping<TClass> MapProperty<TType>(Expression<Func<TClass, TType>> expression)
        {
            this.subMappings.Add(new PropertyMapping<TClass, TType>(expression));
            return this;
        }

        public DomainMapping<TClass> MapProperty<TType>(Expression<Func<TClass, TType>> expression, string columnName)
        {
            this.subMappings.Add(new PropertyMapping<TClass, TType>(expression, columnName));
            return this;
        }

        public DomainMapping<TClass> MapComponent<TComponent>(Expression<Func<TClass, TComponent>> expression, Action<ComponentMapping<TClass, TComponent>> mapping)
        {
            var component = new ComponentMapping<TClass, TComponent>(expression);
            mapping(component);
            
            this.subMappings.Add(component);
            return this;
        }

        public DomainMapping<TClass> MapList<TType>(Expression<Func<TClass, TType>> expression)
        {
            if (!(expression.Body is MemberExpression)) throw new ArgumentException("Only properties can be mapped in this fashion");
            var property = ((MemberExpression)expression.Body).Member as PropertyInfo;

            var listMapping = new ListMapping(property, type, typeof(TClass));
            this.subMappings.Add(listMapping);

            return this;
        }

    }

    public class DomainMapping : IDomainMapping
    {
        protected readonly Type type;
        protected Table table = new Table();
        protected readonly List<IElementMapping> subMappings = new List<IElementMapping>();
        public Table Table { get { return table; } }

        public DomainMapping(Type type)
        {
            this.type = type;
        }

        public List<IElementMapping> SubMappings
        {
            get { return this.subMappings; }
        }

        public Type Type
        {
            get { return type; }
        }

        public DomainMapping MapProperty(PropertyInfo property)
        {
            this.subMappings.Add(new PropertyMapping(property));
            return this;
        }

        public DomainMapping MapComponent(PropertyInfo propertyInfo, Action<ComponentMapping> componentMapping)
        {
            var component = new ComponentMapping(propertyInfo);
            componentMapping(component);
            this.subMappings.Add(component);

            return this;
        }

        public DomainMapping MapID(PropertyInfo property, Formatter<Type> keyFormatter)
        {
            var id = new IdMapping(property, type, keyFormatter);
            this.subMappings.Add(id);

            return this;
        }

        void IDomainMapping.Map(Action<DomainMapping> mapping)
        {
            mapping(this);
        }

        public DomainMapping MapForeignRelationship(DomainMapper masterMap, PropertyInfo property, Formatter<PropertyInfo> keyFormatter)
        {
            var mappedType = masterMap.For(property.PropertyType);
            var foreignKey = mappedType.SubMappings.OfType<IdMapping>().First().Property;
            var foreignMapping = new ForeignRelationshipMapping(property, foreignKey, keyFormatter);
            this.subMappings.Add(foreignMapping);
            return this;
        }

        public DomainMapping MapForeignRelationship(DomainMapper masterMap, PropertyInfo property, Type type, Formatter<PropertyInfo> keyFormatter)
        {
            var mappedType = masterMap.For(type);
            var foreignKey = mappedType.SubMappings.OfType<IdMapping>().First().Property;
            var foreignMapping = new ForeignRelationshipMapping(property, foreignKey, keyFormatter);
            this.subMappings.Add(foreignMapping);
            return this;
        }

        public DomainMapping MapList(DomainMapper masterMap, PropertyInfo property, Type type, Type parentType, Formatter<PropertyInfo> keyFormatter)
        {
            var listMapping = new ListMapping(property, type, parentType, keyFormatter);
            this.subMappings.Add(listMapping);
            return this;
        }

        public void ExportTo(IDialect exporter)
        {
            foreach (var mapping in this.subMappings)
            {
                mapping.ExportTo(exporter);
            }
        }
    }
}