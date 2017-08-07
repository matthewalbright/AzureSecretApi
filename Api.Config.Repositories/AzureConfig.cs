using System;
using System.Threading.Tasks;
using Api.Config.Repositories.Utilities;
using Api.Config.SRServiceLayer.Interfaces;
using Api.Config.SRServiceLayer.Models;

namespace Api.Config.Repositories
{
    public class Azure : IRepository
    {
        public Task<ReturnConfigModel> GetConfig(ConfigModel model)
        {
            try
            {
                var provider = new AzureUtilities(model);
                var response = provider.GetConfig(model);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public Task<ReturnConfigModel> SetConfig(ConfigModel model)
        {
            try
            {
                var provider = new AzureUtilities(model);
                var response = provider.SetConfig(model);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
