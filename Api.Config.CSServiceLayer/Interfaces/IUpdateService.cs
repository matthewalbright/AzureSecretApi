namespace Api.Config.CSServiceLayer.Interfaces
{
    public interface IUpdateService<T> where T : class
    {
        bool Update(string id, T t);
    }
}
