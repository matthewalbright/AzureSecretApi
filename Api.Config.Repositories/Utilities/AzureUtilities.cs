using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Security.Authentication;
using Api.Config.SRServiceLayer.Interfaces;
using Api.Config.SRServiceLayer.Models;
using Microsoft.Rest.Azure.Authentication;

namespace Api.Config.Repositories.Utilities
{
    public class AzureUtilities
    {
        string _clientSecret;
        string _clientId;
        public AzureUtilities(ConfigModel model)
        {
            _clientSecret = model.AuthKey;
            _clientId = model.ClientId;
        }
        public async Task<ReturnConfigModel> GetConfig(ConfigModel model)
        {
            try
            {
                var response = new ReturnConfigModel();
                var secret = await GetSecret(model);
                response.Config = secret;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ReturnConfigModel> SetConfig(ConfigModel model)
        {
            try
            {
                var response = new ReturnConfigModel();
                var secret = await SetSecret(model);
                response.Config = secret;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task<string> GetSecret(ConfigModel model)
        {
            try
            {
                var result = new SecretBundle();
                using (var keyVaultClient = new KeyVaultClient(AuthenticateVault))
                {
                    result = await keyVaultClient.GetSecretAsync(string.Format("https://{0}.vault.azure.net/secrets/{1}", model.VaultName, model.SecretName));
                    keyVaultClient.Dispose();
                };
                return result.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> SetSecret(ConfigModel model)
        {
            try
            {
                var result = new SecretBundle();
                using (var keyVaultClient = new KeyVaultClient(AuthenticateVault))
                {
                    result = await keyVaultClient.SetSecretAsync(string.Format("https://{0}.vault.azure.net/secrets/", model.VaultName), model.SecretName, model.Secret, null, null, CreateSecretAttributes());
                    keyVaultClient.Dispose();
                };
                return result.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SecretAttributes CreateSecretAttributes()
        {
            try
            {
                var attributes = new SecretAttributes()
                {
                    Enabled = true,
                    Expires = DateTime.Now.AddYears(30),
                    NotBefore = DateTime.Now.AddDays(-1),
                };
                return attributes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> AuthenticateVault(string authority, string resource, string scope)
        {
            try
            {
                var clientCredentials = new ClientCredential(_clientId, _clientSecret);
                var authenticationContext = new AuthenticationContext(authority);
                var result = await authenticationContext.AcquireTokenAsync(resource, clientCredentials);
                return result.AccessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
