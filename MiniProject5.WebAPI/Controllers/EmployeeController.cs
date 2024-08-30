using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Persistence.Models;
using MiniProject6.Application.DTOs;
using System.Security.Claims;

namespace MiniProject5.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize(Roles = "Administrator, HR Manager, Department Manager")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees([FromQuery] paginationDto pagination)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(pagination);
            return Ok(employees);
        }

        [Authorize(Roles = "Administrator, HR Manager, Department Manager")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            var newEmployee = await _employeeService.AddEmployeeAsync(employee);
            return Ok(newEmployee);
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPut("{empId}")]
        public async Task<IActionResult> UpdateEmployee(int empId, [FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            await _employeeService.UpdateEmployeeAsync(empId, employee);
            return Ok();
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPut("deactivate/{empId}")]
        public async Task<IActionResult> DeactivateEmployee(int empId, [FromBody] string reason)
        {
            await _employeeService.DeactivateEmployeeAsync(empId, reason);
            return Ok();
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return Ok();
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployee([FromQuery] searchDto search, [FromQuery] paginationDto pagination)
        {
            if (pagination == null || pagination.pageNumber <= 0 || pagination.pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must greater than zero.");
            }

            try
            {
                var employees = await _employeeService.SearchEmployee(search, pagination);

                if (employees == null || !employees.Any())
                {
                    return NotFound("No employees found matching the search criteria.");
                }

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Administrator, Employee Supervisor")]
        [HttpGet("supervisor/{supervisorId}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetSupervisedEmployees(int supervisorId)
        {
            var employees = await _employeeService.GetSupervisedEmployeesAsync(supervisorId);
            if (employees == null || !employees.Any())
            {
                return NotFound("No supervised employees found.");
            }
            return Ok(employees);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetOwnProfile()
        {
            var profile = await _employeeService.GetOwnProfile();

            if (profile == null)
            {
                return NotFound("Profil karyawan tidak ditemukan.");
            }

            return Ok(profile);
        }


        [Authorize(Roles = "Employee")]
        [HttpPut("update/{empId}")]
        public async Task<IActionResult> UpdateOwnProfile(int empId, [FromBody] EmployeeDto employeeDto)
        {
            try
            {
                await _employeeService.UpdateOwnProfile(empId, employeeDto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
