using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Application.Services;
using MiniProject5.Persistence.Models;

namespace MiniProject5.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [Authorize(Roles = "Administrator, Department Manager")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments([FromQuery] paginationDto pagination)
        {
            var departments = await _departmentService.GetAllDepartmentsAsync(pagination);
            return Ok(departments);
        }

        [Authorize(Roles = "Administrator, Department Manager")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Department>> AddDepartment(Department department)
        {
            var newDepartment = await _departmentService.AddDepartmentAsync(department);
            return CreatedAtAction(nameof(GetDepartment), new { id = newDepartment.Deptid }, newDepartment);
        }

        [Authorize(Roles = "Administrator, Department Manager")]
        [HttpPut("{deptId}")]
        public async Task<IActionResult> UpdateDepartment(int deptId, [FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest();
            }

            await _departmentService.UpdateDepartmentAsync(deptId, department);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{deptId}")]
        public async Task<IActionResult> DeleteDepartment(int deptId)
        {
            await _departmentService.DeleteDepartmentAsync(deptId);
            return Ok();
        }
    }
}
