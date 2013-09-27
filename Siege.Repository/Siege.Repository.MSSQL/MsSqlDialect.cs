using System;
using System.Collections.Generic;
using System.Reflection;
using Siege.Repository.Mapping;
using Siege.Repository.Mapping.PropertyMappings;

namespace Siege.Repository.MSSQL
{
    public class MsSqlDialect : IDialect
    {
        private readonly ISqlExporter exporter;

        public MsSqlDialect(ISqlExporter exporter)
        {
            this.exporter = exporter;
        }

        public void CreateTable(Table table, Action<IDialect> tableCreater)
        {
            exporter.Export(string.Format("CREATE TABLE [{0}].[{1}]", table.Schema, table.Name));
            exporter.Export("(");
            tableCreater(this);
            exporter.Export(")");
            exporter.Export("");
        }

        public void CreateField(IPropertyMapping mapping)
        {
            exporter.Export(string.Format("\t{0} {1},", mapping.ColumnName, SqlDataTypeFor(mapping.Property.PropertyType)));
        }

        public void CreateForeignKey(ForeignRelationshipMapping mapping, PropertyInfo id)
        {
            exporter.Export(string.Format("\t{0} {1},", mapping.ColumnName, SqlDataTypeFor(id.PropertyType)));
        }

        private string SqlDataTypeFor(Type type)
        {
            if (type == typeof (string))
            {
                return "nvarchar(50)";
            }

            if (Supports<decimal>(type))
            {
                return "decimal";
            }

            if (Supports<int>(type))
            {
                return "int";
            }

            if (Supports<DateTime>(type))
            {
                return "DateTime";
            }

            if (Supports<Guid>(type))
            {
                return "uniqueidentifier";
            }

            throw new NotSupportedException(string.Format("The data type {0} is not supported.", type));
        }

        private bool Supports<TType>(Type type) where TType : struct
        {
            return type == typeof (TType) || type == typeof (TType?);
        }
    }
}