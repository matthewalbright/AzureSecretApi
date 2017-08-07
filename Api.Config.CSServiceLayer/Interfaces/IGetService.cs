using Api.Config.CSServiceLayer.Models;
using System.Collections.Generic;

namespace Api.Config.CSServiceLayer.Interfaces
{
    public interface IGetService<T> where T : class
    {
        T GetConfig(ConfigModel model);
        T SetConfig(ConfigModel model);
    }
}
