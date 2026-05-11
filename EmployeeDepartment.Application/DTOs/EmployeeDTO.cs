using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDepartment.Application.DTOs
{
    public class EmployeeDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
    }
}
