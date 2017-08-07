using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Config.CSServiceLayer.Models
{
    public class ConfigModel
    {
        public string VaultName { get; set; }
        public string SecretName { get; set; } 
        public string AuthKey { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
    }
}
