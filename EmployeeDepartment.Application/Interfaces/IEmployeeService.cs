using EmployeeDepartment.Application.DTOs;
using EmployeeDepartment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDepartment.Application.Interfaces
{
    public interface IEmployeeService
    {
        ResponseDto<List<Employee>> GetAllEmployees();
        ResponseDto<Employee?> GetEmployeeById(int id);
        ResponseDto<string> AddEmployee(EmployeeDTO employeeDto);
        ResponseDto<string> UpdateEmployee(int id, EmployeeDTO employeeDto);
        ResponseDto<string> DeleteEmployee(int id);
    }
}
