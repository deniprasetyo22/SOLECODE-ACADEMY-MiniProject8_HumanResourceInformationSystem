using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProject8.Application.DTOs;
using MiniProject8.Application.Interfaces.IServices;
using MiniProject8.Domain.Models;

namespace MiniProject8.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorksOnController : ControllerBase
    {
        private readonly IWorksOnService _worksOnService;

        public WorksOnController(IWorksOnService worksOnService)
        {
            _worksOnService = worksOnService;
        }

        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee Supervisor, Employee")]
        [HttpGet]
        public async Task<IActionResult> GetAllWorksOn([FromQuery] paginationDto pagination)
        {
            try
            {
                var worksOnList = await _worksOnService.GetAllWorksOnAsync(pagination);
                return Ok(worksOnList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving works on.", error = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee Supervisor, Employee")]
        [HttpGet("nopages")]
        public async Task<IActionResult> GetAllWorksOnNoPagesAsync()
        {
            try
            {
                var worksOnList = await _worksOnService.GetAllWorksOnNoPagesAsync();
                return Ok(worksOnList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving works on.", error = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator, Employee, HR Manager, Employee Supervisor, Employee")]
        [HttpGet("{empId}/{projId}")]
        public async Task<IActionResult> GetWorksOn(int empId, int projId)
        {
            try
            {
                var worksOn = await _worksOnService.GetWorksOnByIdAsync(empId, projId);
                if (worksOn == null)
                {
                    return NotFound(new { message = $"Workson with Employee ID {empId} and Project ID {projId} not found" });
                }
                return Ok(worksOn);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator, Employee Supervisor")]
        [HttpPost]
        public async Task<IActionResult> AddWorksOn(int empId, int projId, Workson worksOn)
        {
            try
            {
                var newWorksOn = await _worksOnService.AddWorksOnAsync(empId, projId, worksOn);
                return Ok(new { message = "Workson added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator, Employee Supervisor")]
        [HttpPut("{empId}/{projId}")]
        public async Task<IActionResult> UpdateWorksOn(int empId, int projId, [FromBody] Workson worksOn)
        {
            try
            {
                var updatedWorkson = _worksOnService.UpdateWorksOnAsync(empId, projId, worksOn);
                return Ok(new { message = "Workson updated successfully.", data = updatedWorkson });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrator, Employee Supervisor")]
        [HttpDelete("{empId}/{projId}")]
        public async Task<IActionResult> DeleteWorksOn(int empId, int projId)
        {
            try
            {
                await _worksOnService.DeleteWorksOnAsync(empId, projId);
                return Ok(new {message = "Workson deleted successfully."});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator, Employee, Employee Supervisor")]
        [HttpGet("ownWorkson")]
        public async Task<IActionResult> GetOwnWorkson()
        {
            try
            {
                var ownWorkson = await _worksOnService.GetOwnWorksonAsync();
                return Ok(ownWorkson);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
