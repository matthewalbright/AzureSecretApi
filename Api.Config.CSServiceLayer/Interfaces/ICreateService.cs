namespace Api.Config.CSServiceLayer.Interfaces
{
    public interface ICreateService<T> where T : class
    {
        T Create(T t);
    }
}
