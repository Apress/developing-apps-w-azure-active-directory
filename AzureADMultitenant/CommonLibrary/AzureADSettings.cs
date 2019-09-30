using System;

namespace CommonLibrary
{
    public class AzureADSettings
    {
        public string Instance { get; set; }

        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string ResourceId { get; set; }

        public string Authority => $"{this.Instance}{this.TenantId}";

        public string CallbackPath { get; set; }

        public string ResourceBasePath { get; set; }

        public static AzureADSettings AzureSettings { get; set; }
    }
}
