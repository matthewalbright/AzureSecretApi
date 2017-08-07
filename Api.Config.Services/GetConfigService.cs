using Api.Config.CSServiceLayer.Interfaces;
using Api.Config.CSServiceLayer.Models;
using Api.Config.SRServiceLayer.Interfaces;
using Api.Config.SRServiceLayer.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Config.Services
{
    public class GetConfigService : IConfigService
    {
        private readonly ILogger _logger;
        private readonly IRepository _configRepository;
        public GetConfigService(ILogger logger, IRepository configRepository)
        {
            _logger = logger;
            _configRepository = configRepository;
        }

        public CSServiceLayer.Models.ReturnConfigModel GetConfig(CSServiceLayer.Models.ConfigModel model)
        {
            try {
                var srModel = new SRServiceLayer.Models.ConfigModel()
                {
                    AuthKey = model.AuthKey,
                    SecretName = model.SecretName,
                    VaultName = model.VaultName,
                    ClientId = model.ClientId
                };

                var response = _configRepository.GetConfig(srModel);
                Task.WaitAny(response);
                CSServiceLayer.Models.ReturnConfigModel returnedConfig = new CSServiceLayer.Models.ReturnConfigModel()
                {
                    Config = response.Result.Config
                };
                return returnedConfig;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(2, "ConfigError"), ex.StackTrace, new object[] { });
                throw ex;
            }

        }

        public CSServiceLayer.Models.ReturnConfigModel SetConfig(CSServiceLayer.Models.ConfigModel model)
        {
            try
            {
                var srModel = new SRServiceLayer.Models.ConfigModel()
                {
                    AuthKey = model.AuthKey,
                    SecretName = model.SecretName,
                    VaultName = model.VaultName,
                    ClientId = model.ClientId,
                    Secret = model.Secret
                };

                var response = _configRepository.SetConfig(srModel);
                Task.WaitAny(response);
                CSServiceLayer.Models.ReturnConfigModel returnedConfig = new CSServiceLayer.Models.ReturnConfigModel()
                {
                    Config = response.Result.Config
                };
                return returnedConfig;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(2, "ConfigError"), ex.StackTrace, new object[] { });
                throw ex;
            }

        }


    }
}
