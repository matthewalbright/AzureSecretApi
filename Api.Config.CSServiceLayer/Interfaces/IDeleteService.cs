namespace Api.Config.CSServiceLayer.Interfaces
{
    public interface IDeleteService<T> where T : class
    {
        bool Delete(string id);
    }
}
