using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Config.Startup.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Api.Config.Startup.Authorization
{
    public static class AuthorizationSetup
    {
        public static JwtBearerOptions GetJwtBearerOptions(AuthorizationSettings authSettings, ILogger logger)
        {
            var keyAsBase64 = authSettings.TokenKeyInBase64.Replace('_', '/').Replace('-', '+');
            var keyAsBytes = Convert.FromBase64String(keyAsBase64);

            var options = new JwtBearerOptions
            {
                AutomaticChallenge = true,
                AutomaticAuthenticate = true,
                TokenValidationParameters =
                {
                    IssuerSigningKey = new SymmetricSecurityKey(keyAsBytes),
                    //Audience must be populated, even though not used for validations
                    //since we populate with our own ClientId
                    ValidAudience = authSettings.Audience,
                    ValidateAudience = false,
                    ValidIssuer = authSettings.Issuer
                },
                Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // TODO: Tie in Exceptionless with logger.
                        logger.LogError("Authentication failed.", context.Exception);
                        return Task.FromResult(0);
                    },

                    OnTokenValidated = context =>
                    {
                        var claimsIdentity = context.Ticket.Principal.Identity as ClaimsIdentity;

                        var authSchemeLength = context.Ticket.AuthenticationScheme.Length + 1;
                        var authHeader = context.Request.Headers["Authorization"][0];

                        var token = authHeader.Substring(authSchemeLength);
                        if (string.IsNullOrEmpty(token))
                        {
                            return Task.FromResult(0);
                        }


                        //get the unecrypted JWT token and decode it and extract the claims section
                        var notPadded = token.Split('.')[1];
                        var padded = notPadded.PadRight(notPadded.Length + (4 - notPadded.Length % 4) % 4, '=');
                        var urlUnescaped = padded.Replace('-', '+').Replace('_', '/');
                        var claimsPart = Convert.FromBase64String(urlUnescaped);
                        var obj = JObject.Parse(Encoding.UTF8.GetString(claimsPart, 0, claimsPart.Length));
                        //string json = Jose.JWT.Decode(result);

                        return Task.FromResult(0);
                    }
                }
            };

            return options;
        }

        private static void AddClaim(ClaimsIdentity claimsIdentity, string claimName, string claimValue)
        {
            if (claimValue != null)
            {
                claimsIdentity?.AddClaim(new Claim(claimName, claimValue));
            }
        }
    }
}
