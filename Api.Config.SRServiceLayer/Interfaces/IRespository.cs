using Api.Config.SRServiceLayer.Models;
using System.Threading.Tasks;

namespace Api.Config.SRServiceLayer.Interfaces
{
    public interface IRepository
    {
        Task<ReturnConfigModel> GetConfig(ConfigModel model);
        Task<ReturnConfigModel> SetConfig(ConfigModel model);
    }
}
