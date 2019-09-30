using AuthorizationDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationDemo.Services
{
    public interface IMockEmployeeService
    {
        EmployeeEntity CreateMockEmployee(int employeeId);
    }
}
