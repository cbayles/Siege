using System.Text;
using Siege.Repository.Mapping;

namespace Siege.Repository.MSSQL
{
    public class SqlExporter : ISqlExporter
    {
        private readonly StringBuilder builder = new StringBuilder();
        private readonly IDialect dialect;

        public SqlExporter()
        {
            this.dialect = new MsSqlDialect(this);
        }

        public string Export(DomainMap map)
        {
            foreach (var type in map.Mappings.MappedTypes)
            {
                var mapping = map.Mappings.For(type);
                dialect.CreateTable(mapping.Table, x => mapping.ExportTo(dialect));
            }

            return builder.ToString();
        }


        public void Export(string sql)
        {
            builder.AppendLine(sql);
        }
    }
}
