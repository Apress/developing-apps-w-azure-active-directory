using AuthorizationDemo.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationDemo.Authorization.AuthorizationHandlers
{
    public class EmployeeDepartmentHandler : AuthorizationHandler<EmployeeDepartmentRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeDepartmentRequirement requirement)
        {
            // Logic to check the department of the employee
            if (context.User.HasClaim("extn.employeeDepartment", requirement.DepartmentName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
