using System.Collections.Generic;

namespace Api.Config.Startup.Authorization
{
    public class AuthorizationSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string TokenKeyInBase64 { get; set; }
    }
}
