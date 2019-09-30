using System;
using System.Collections.Generic;

namespace MultitenantAPI.DAL
{
    public partial class Employees
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Department { get; set; }
    }
}
