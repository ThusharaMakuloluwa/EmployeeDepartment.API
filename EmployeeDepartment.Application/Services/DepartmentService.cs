using EmployeeDepartment.Application.DTOs;
using EmployeeDepartment.Application.Interfaces;
using EmployeeDepartment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDepartment.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public ResponseDto<List<Department>> GetAllDepartments()
        {
            try
            {
                var departments = _departmentRepository.GetAll();
                return ResponseDto<List<Department>>.SuccessResponse(departments);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<Department>>.ErrorResponse(ex.Message);
            }
        }

        public ResponseDto<Department?> GetDepartmentById(int id)
        {
            try
            {
                var department = _departmentRepository.GetById(id);
                if (department == null)
                    return ResponseDto<Department?>.ErrorResponse("Department not found.");
                return ResponseDto<Department?>.SuccessResponse(department);
            }
            catch (Exception ex)
            {
                return ResponseDto<Department?>.ErrorResponse(ex.Message);
            }
        }

        public ResponseDto<string> AddDepartment(DepartmentDTO departmentDto)
        {
            if (string.IsNullOrWhiteSpace(departmentDto.DepartmentCode))
                return ResponseDto<string>.ErrorResponse("Department Code is required.");

            if (string.IsNullOrWhiteSpace(departmentDto.DepartmentName))
                return ResponseDto<string>.ErrorResponse("Department Name is required.");

            try
            {
                var department = new Department
                {
                    DepartmentCode = departmentDto.DepartmentCode,
                    DepartmentName = departmentDto.DepartmentName
                };

                _departmentRepository.Add(department);
                return ResponseDto<string>.SuccessResponse("Department added successfully.");
            }
            catch (Exception ex)
            {
                return ResponseDto<string>.ErrorResponse(ex.Message);
            }
        }

        public ResponseDto<string> UpdateDepartment(int id, DepartmentDTO departmentDto)
        {
            if (string.IsNullOrWhiteSpace(departmentDto.DepartmentCode))
                return ResponseDto<string>.ErrorResponse("Department Code is required.");

            if (string.IsNullOrWhiteSpace(departmentDto.DepartmentName))
                return ResponseDto<string>.ErrorResponse("Department Name is required.");

            try
            {
                var department = new Department
                {
                    DepartmentId = id,
                    DepartmentCode = departmentDto.DepartmentCode,
                    DepartmentName = departmentDto.DepartmentName
                };

                _departmentRepository.Update(department);
                return ResponseDto<string>.SuccessResponse("Department updated successfully.");
            }
            catch (Exception ex)
            {
                return ResponseDto<string>.ErrorResponse(ex.Message);
            }
        }

        public ResponseDto<string> DeleteDepartment(int id)
        {
            try
            {
                _departmentRepository.Delete(id);
                return ResponseDto<string>.SuccessResponse("Department deleted successfully.");
            }
            catch (Exception ex)
            {
                return ResponseDto<string>.ErrorResponse(ex.Message);
            }
        }
    }
}

