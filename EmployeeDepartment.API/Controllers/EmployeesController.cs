using EmployeeDepartment.Application.DTOs;
using EmployeeDepartment.Application.Interfaces;
using EmployeeDepartment.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDepartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_employeeService.GetAllEmployees());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _employeeService.GetEmployeeById(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(EmployeeDTO employeeDto)
        {
            var result = _employeeService.AddEmployee(employeeDto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, EmployeeDTO employeeDto)
        {
            var result = _employeeService.UpdateEmployee(id, employeeDto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _employeeService.DeleteEmployee(id);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
