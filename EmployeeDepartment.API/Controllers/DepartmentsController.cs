using EmployeeDepartment.Application.DTOs;
using EmployeeDepartment.Application.Interfaces;
using EmployeeDepartment.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDepartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_departmentService.GetAllDepartments());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _departmentService.GetDepartmentById(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(DepartmentDTO departmentDto)
        {
            var result = _departmentService.AddDepartment(departmentDto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, DepartmentDTO departmentDto)
        {
            var result = _departmentService.UpdateDepartment(id, departmentDto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _departmentService.DeleteDepartment(id);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
