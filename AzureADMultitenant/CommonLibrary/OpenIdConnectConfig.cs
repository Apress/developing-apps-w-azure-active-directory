using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonLibrary
{
    internal class OpenIdConnectConfig : IConfigureNamedOptions<OpenIdConnectOptions>
    {
        private readonly AzureADSettings azureADSettings;

        private readonly List<string> validIssuers = new List<string>()
        {
            "https://sts.windows.net/ce3e874c-a6b4-4300-807e-00d6db764856/",
            "https://sts.windows.net/9ffc2d15-ffdd-4a44-9f6b-1b11df8bb417/"
        };

        public OpenIdConnectConfig(IOptions<AzureADSettings> azureADSettings)
        {
            this.azureADSettings = azureADSettings.Value;
        }

        public void Configure(string name, OpenIdConnectOptions options)
        {
            options.ClientId = azureADSettings.ClientId;
            // Single tenant authority
            // options.Authority = azureADSettings.Authority;
            // Multi-tenant authority can not be specific to a tenant instance, and should point to https://login.microsoftonline.com/common
            options.Authority = "https://login.microsoftonline.com/common";
            options.UseTokenLifetime = true;
            options.CallbackPath = azureADSettings.CallbackPath;
            options.RequireHttpsMetadata = false;
            options.ClientSecret = azureADSettings.ClientSecret;
            options.Resource = azureADSettings.ResourceId;
            // Set ValidateIssuer to support multitenancy
            options.TokenValidationParameters.ValidateIssuer = true;
            options.TokenValidationParameters.IssuerValidator = ValidateIssuer;
            // Without overriding the response type (which by default is id_token), the OnAuthorizationCodeReceived event is not called.
            // but instead OnTokenValidated event is called. Here we request both so that OnTokenValidated is called first which 
            // ensures that context.Principal has a non-null value when OnAuthorizeationCodeReceived is called
            options.ResponseType = "id_token code";

            // Subscribing to the OIDC events
            options.Events.OnAuthorizationCodeReceived = OnAuthorizationCodeReceived;
            options.Events.OnAuthenticationFailed = OnAuthenticationFailed;
            options.Events.OnTokenValidated = TokenValidate;
        }

        private Task TokenValidate(TokenValidatedContext arg)
        {
            int i = 0;
            return Task.CompletedTask;
        }

        private string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (validIssuers.Contains(issuer?.ToLowerInvariant()))
            {
                return issuer;
            }

            throw new SecurityTokenInvalidIssuerException($"Invalid tenant: {issuer}")
            {
                InvalidIssuer = issuer
            };
        }

        public void Configure(OpenIdConnectOptions options)
        {
            Configure(Options.DefaultName, options);
        }

        private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            string userObjectId = (context.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            var authContext = new AuthenticationContext(context.Options.Authority, new TokenSessionCache(userObjectId, context.HttpContext.Session));
            var credential = new ClientCredential(context.Options.ClientId, context.Options.ClientSecret);
            var authResult = await authContext.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                new Uri(context.TokenEndpointRequest.RedirectUri, UriKind.RelativeOrAbsolute), credential, context.Options.Resource);

            // Notify the OIDC middleware that we already took care of code redemption.
            context.HandleCodeRedemption(authResult.AccessToken, context.ProtocolMessage.IdToken);
        }

        /// <summary>
        /// this method is invoked if exceptions are thrown during request processing
        /// </summary>
        private Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            context.HandleResponse();
            context.Response.Redirect("/Home/Error?message=" + context.Exception.Message);
            return Task.FromResult(0);
        }
    }
}