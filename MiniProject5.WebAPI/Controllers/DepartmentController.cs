using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProject8.Application.DTOs;
using MiniProject8.Application.Interfaces.IServices;
using MiniProject8.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace MiniProject8.WebAPI.Controllers
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

        // GET: api/Department
        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee Supervisor, Employee")]
        [HttpGet]
        public async Task<ActionResult> GetAllDepartments([FromQuery] QueryObjectDepartment query)
        {
            var result = await _departmentService.GetAllDepartmentsAsync(query);
            return Ok(result);
        }

        // GET: api/Department/NoPages
        [HttpGet("NoPages")]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartmentsNoPages()
        {
            var departments = await _departmentService.GetAllDepartmentsNoPagesAsync();
            return Ok(departments);
        }

        // GET: api/Department/{id}
        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee Supervisor, Employee")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id);
                return Ok(department);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/Department
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Department>> AddDepartment([FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest(new { message = "Invalid department data." });
            }

            try
            {
                var newDepartment = await _departmentService.AddDepartmentAsync(department);
                return CreatedAtAction(nameof(GetDepartment), new { id = newDepartment.Deptid }, newDepartment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Department/{deptId}
        [Authorize(Roles = "Administrator, Department Manager")]
        [HttpPut("{deptId}")]
        public async Task<IActionResult> UpdateDepartment(int deptId, [FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest(new { message = "Invalid department data." });
            }

            try
            {
                await _departmentService.UpdateDepartmentAsync(deptId, department);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/Department/{deptId}
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{deptId}")]
        public async Task<IActionResult> DeleteDepartment(int deptId)
        {
            try
            {
                await _departmentService.DeleteDepartmentAsync(deptId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
