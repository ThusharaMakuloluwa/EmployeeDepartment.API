using EmployeeDepartment.Application.DTOs;
using EmployeeDepartment.Application.Interfaces;
using EmployeeDepartment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDepartment.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public ResponseDto<List<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = _employeeRepository.GetAll();
                return ResponseDto<List<Employee>>.SuccessResponse(employees);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<Employee>>.ErrorResponse(ex.Message);
            }
        }

        public ResponseDto<Employee?> GetEmployeeById(int id)
        {
            try
            {
                var employee = _employeeRepository.GetById(id);
                if (employee == null)
                    return ResponseDto<Employee?>.ErrorResponse("Employee not found.");
                return ResponseDto<Employee?>.SuccessResponse(employee);
            }
            catch (Exception ex)
            {
                return ResponseDto<Employee?>.ErrorResponse(ex.Message);
            }
        }

        public ResponseDto<string> AddEmployee(EmployeeDTO employeeDto)
        {
            var validationMessage = ValidateEmployee(employeeDto);
            if (validationMessage != "Valid")
                return ResponseDto<string>.ErrorResponse(validationMessage);

            try
            {
                var employee = new Employee
                {
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    EmailAddress = employeeDto.EmailAddress,
                    DateOfBirth = employeeDto.DateOfBirth,
                    Salary = employeeDto.Salary,
                    DepartmentId = employeeDto.DepartmentId
                };

                _employeeRepository.Add(employee);
                return ResponseDto<string>.SuccessResponse("Employee added successfully.");
            }
            catch (Exception ex)
            {
                return ResponseDto<string>.ErrorResponse(ex.Message);
            }
        }

        public ResponseDto<string> UpdateEmployee(int id, EmployeeDTO employeeDto)
        {
            var validationMessage = ValidateEmployee(employeeDto);
            if (validationMessage != "Valid")
                return ResponseDto<string>.ErrorResponse(validationMessage);

            try
            {
                var employee = new Employee
                {
                    EmployeeId = id,
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    EmailAddress = employeeDto.EmailAddress,
                    DateOfBirth = employeeDto.DateOfBirth,
                    Salary = employeeDto.Salary,
                    DepartmentId = employeeDto.DepartmentId
                };

                _employeeRepository.Update(employee);
                return ResponseDto<string>.SuccessResponse("Employee updated successfully.");
            }
            catch (Exception ex)
            {
                return ResponseDto<string>.ErrorResponse(ex.Message);
            }
        }

        public ResponseDto<string> DeleteEmployee(int id)
        {
            try
            {
                _employeeRepository.Delete(id);
                return ResponseDto<string>.SuccessResponse("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return ResponseDto<string>.ErrorResponse(ex.Message);
            }
        }

        private string ValidateEmployee(EmployeeDTO employeeDto)
        {
            if (string.IsNullOrWhiteSpace(employeeDto.FirstName))
                return "First Name is required.";

            if (string.IsNullOrWhiteSpace(employeeDto.LastName))
                return "Last Name is required.";

            if (string.IsNullOrWhiteSpace(employeeDto.EmailAddress))
                return "Email Address is required.";

            if (!employeeDto.EmailAddress.Contains("@"))
                return "Please enter a valid email address.";

            if (employeeDto.DateOfBirth == default)
                return "Date of Birth is required.";

            if (employeeDto.Salary <= 0)
                return "Salary must be greater than 0.";

            if (employeeDto.DepartmentId <= 0)
                return "Department is required.";

            return "Valid";
        }
    }
}
