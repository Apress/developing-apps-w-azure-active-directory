using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultitenantAPI.DAL
{
    public class TenantConnectionMapper
    {
        // Dictionary holding the mapping between and AD tenant and corresponding connection string. 
        private readonly Dictionary<string, string> connectionMapper = new Dictionary<string, string>();

        public TenantConnectionMapper()
        {
            connectionMapper.Add("ce3e874c-a6b4-4300-807e-00d6db764856", "Server=.;Database=EnterpriseDB;Trusted_Connection=True;");
            connectionMapper.Add("9ffc2d15-ffdd-4a44-9f6b-1b11df8bb417", "Server=.;Database=PartnerDB;Trusted_Connection=True;");
        }

        // This is for demo purpose. In real world scenario this method will get connection string from a secure persistence.
        public string GetConnectionString(string tenant)
        {
            if (connectionMapper.Keys.Contains(tenant))
            {
                return connectionMapper[tenant];
            }

            return string.Empty;
        }
    }
}
