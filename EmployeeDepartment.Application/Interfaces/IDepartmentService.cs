using EmployeeDepartment.Application.DTOs;
using EmployeeDepartment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDepartment.Application.Interfaces
{
    public interface IDepartmentService
    {
        ResponseDto<List<Department>> GetAllDepartments();
        ResponseDto<Department?> GetDepartmentById(int id);
        ResponseDto<string> AddDepartment(DepartmentDTO departmentDto);
        ResponseDto<string> UpdateDepartment(int id, DepartmentDTO departmentDto);
        ResponseDto<string> DeleteDepartment(int id);
    }
}
