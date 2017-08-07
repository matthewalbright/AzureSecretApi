namespace Api.Config.CSServiceLayer.Interfaces
{
    public interface IService<T> : IGetService<T> where T : class
    {
    }
}
