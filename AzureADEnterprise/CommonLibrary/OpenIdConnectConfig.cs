using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace CommonLibrary
{
    internal class OpenIdConnectConfig : IConfigureNamedOptions<OpenIdConnectOptions>
    {
        private readonly AzureADSettings azureADSettings;

        public OpenIdConnectConfig(IOptions<AzureADSettings> azureADSettings)
        {
            this.azureADSettings = azureADSettings.Value;
        }

        public void Configure(string name, OpenIdConnectOptions options)
        {
            options.ClientId = azureADSettings.ClientId;
            options.Authority = azureADSettings.Authority;
            options.UseTokenLifetime = true;
            options.CallbackPath = azureADSettings.CallbackPath;
            options.RequireHttpsMetadata = false;
            options.ClientSecret = azureADSettings.ClientSecret;
            options.Resource = azureADSettings.ResourceId;
            options.ResponseType = "id_token code";

            // Subscribing to the OIDC events
            options.Events.OnAuthorizationCodeReceived = OnAuthorizationCodeReceived;
            options.Events.OnAuthenticationFailed = OnAuthenticationFailed;
            options.Events.OnTokenValidated = TokenValidated;
        }

        private Task TokenValidated(TokenValidatedContext arg)
        {
            return Task.CompletedTask;
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