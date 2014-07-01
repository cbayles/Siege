namespace Siege.Repository.Commands
{
    public interface ICommand
    {
        T Execute<T>();
    }
}
