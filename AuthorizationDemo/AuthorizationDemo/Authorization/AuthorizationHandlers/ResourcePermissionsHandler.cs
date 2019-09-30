using AuthorizationDemo.Authorization.Requirements;
using AuthorizationDemo.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationDemo.Authorization.AuthorizationHandlers
{
    public class ResourcePermissionsHandler :
        AuthorizationHandler<ResourcePermissionsRequirement, EmployeeEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourcePermissionsRequirement requirement, EmployeeEntity employee)
        {
            string employeeIdFromClaims = context.User.Claims.FirstOrDefault(c => c.Type == "EmployeeId").Value;
            if (context.User.HasClaim(requirement.Resource, requirement.Permission) || employee.EmployeeId.ToString() == employeeIdFromClaims)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
