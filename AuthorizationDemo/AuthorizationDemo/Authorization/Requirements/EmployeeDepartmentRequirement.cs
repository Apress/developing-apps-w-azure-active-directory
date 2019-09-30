using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationDemo.Authorization.Requirements
{
    public class EmployeeDepartmentRequirement : IAuthorizationRequirement
    {
        public string DepartmentName { get; set; }

        public EmployeeDepartmentRequirement(string departmentName)
        {
            this.DepartmentName = departmentName;
        }
    }
}
