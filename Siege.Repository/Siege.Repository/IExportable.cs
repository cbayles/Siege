namespace Siege.Repository
{
    public interface IExportable
    {
        void ExportTo(IDialect exporter);
    }
}