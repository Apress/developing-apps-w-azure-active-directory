using AuthorizationDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationDemo.Services
{
    // Logic mimicking getting employee details. In real world, this will come from 
    public class MockEmployeeService : IMockEmployeeService
    {
        public EmployeeEntity CreateMockEmployee(int employeeId)
        {
            if (employeeId == 1)
            {
                return new EmployeeEntity() { Department = "HR", Designation = "Manager", EmployeeId = 1, EmployeeName = "HR Sharma" };
            }
            else
            {
                return new EmployeeEntity() { Department = "FINANCE", Designation = "Accountant", EmployeeId = employeeId, EmployeeName = "FI Nance" };
            }
        }
    }
}
