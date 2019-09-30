using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;

namespace CommonLibrary
{
    public static class AzureAdAuthExtension
    {
        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder, Action<AzureADSettings> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, OpenIdConnectConfig>();
            builder.AddOpenIdConnect();
            return builder;
        }
    }
}
