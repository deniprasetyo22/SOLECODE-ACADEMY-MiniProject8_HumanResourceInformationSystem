using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProject8.Application.DTOs;
using MiniProject8.Application.Interfaces.IServices;
using MiniProject8.Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MiniProject8.WebAPI.Controllers
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

        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee Supervisor, Employee")]
        [HttpGet]
        public async Task<ActionResult<object>> GetAllEmployees([FromQuery] QueryObjectEmployee query)
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync(query);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee Supervisor, Employee")]
        [HttpGet("NoPages")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployeesNoPages()
        {
            var employees = await _employeeService.GetAllEmployeesNoPagesAsync();
            return Ok(employees);
        }


        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee Supervisor")]
        [HttpGet("{empId}")]
        public async Task<ActionResult<Employee>> GetEmployee(int empId)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(empId);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                var newEmployee = await _employeeService.AddEmployeeAsync(employee);
                return Ok("Employee created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPut("{empId}")]
        public async Task<IActionResult> UpdateEmployee(int empId, [FromBody] EmployeeDto employeeDto)
        {
            try
            {
                await _employeeService.UpdateEmployeeAsync(empId, employeeDto);
                return Ok(new { Message = "Employee Updated Successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPut("deactivate/{empId}")]
        public async Task<IActionResult> DeactivateEmployee(int empId, [FromBody] string reason)
        {
            try
            {
                await _employeeService.DeactivateEmployeeAsync(empId, reason);
                return Ok(new { Message = "Employee deactived successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPut("activate/{empId}")]
        public async Task<IActionResult> ActivateEmployee(int empId)
        {
            try
            {
                await _employeeService.ActivateEmployeeAsync(empId);
                return Ok(new { Message = "Employee activate successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
                return Ok(new { Message = "Employee deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Administrator, HR Manager, Employee Supervisor")]
        [HttpGet("search")]
        public async Task<ActionResult<object>> SearchEmployee([FromQuery] searchDto search, [FromQuery] paginationDto pagination)
        {
            if (pagination == null || pagination.pageNumber <= 0 || pagination.pageSize <= 0)
            {
                return BadRequest(new { Message = "PageNumber and PageSize must be greater than zero." });
            }

            try
            {
                var employees = await _employeeService.SearchEmployee(search, pagination);

                if (employees == null || !employees.Any())
                {
                    return NotFound(new { Message = "No employees found matching the search criteria." });
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
                return NotFound(new { Message = "No supervised employees found." });
            }
            return Ok(employees);
        }

        [Authorize(Roles = "Administrator, Employee")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetOwnProfile()
        {
            var profile = await _employeeService.GetOwnProfile();

            if (profile == null)
            {
                return NotFound(new { Message = "Profile Not Found." });
            }

            return Ok(profile);
        }


        [Authorize(Roles = "Administrator, Employee")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateOwnProfile([FromBody] EmployeeDto employeeDto)
        {
            var updateOwnPtofile = await _employeeService.UpdateOwnProfile(employeeDto);
            try
            {
                return Ok(new { Message = "Profile updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Profile Not Found." });
            }
        }
    }
}
