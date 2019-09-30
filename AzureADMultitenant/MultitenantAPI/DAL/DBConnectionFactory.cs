using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MultitenantAPI.DAL
{
    public class DBConnectionFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly TenantConnectionMapper tenantConnectionMapper;

        public DBConnectionFactory(IHttpContextAccessor httpContextAccessor, TenantConnectionMapper tenantConnectionMapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.tenantConnectionMapper = tenantConnectionMapper;
        }

        public DbConnection GetDbConnection()
        {
            var tenantId = httpContextAccessor.HttpContext.User.Claims
                          .FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
            string connectionString = this.tenantConnectionMapper.GetConnectionString(tenantId);
            return new SqlConnection(connectionString);
        }
    }
}
