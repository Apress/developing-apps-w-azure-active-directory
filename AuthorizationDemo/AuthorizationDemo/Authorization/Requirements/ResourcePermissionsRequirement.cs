using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationDemo.Authorization.Requirements
{
    public class ResourcePermissionsRequirement : IAuthorizationRequirement
    {
        public string Resource { get; set; }
        public string Permission { get; set; }

        public ResourcePermissionsRequirement(string resource, string permission)
        {
            this.Resource = resource;
            this.Permission = permission;
        }
    }
}
