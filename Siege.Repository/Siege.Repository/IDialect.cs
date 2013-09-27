using System;
using System.Reflection;
using Siege.Repository.Mapping;
using Siege.Repository.Mapping.PropertyMappings;

namespace Siege.Repository
{
    public interface IDialect
    {
        void CreateTable(Table table, Action<IDialect> tableCreater);
        void CreateField(IPropertyMapping mapping);
        void CreateForeignKey(ForeignRelationshipMapping foreignRelationshipMapping, PropertyInfo id);
    }
}
