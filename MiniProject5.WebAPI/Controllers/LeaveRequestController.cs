using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject8.Application.DTOs;
using MiniProject8.Application.Interfaces.IServices;
using MiniProject8.Domain.Models;

namespace MiniProject8.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequest;
        private readonly IWebHostEnvironment _environment;
        public LeaveRequestController(ILeaveRequestService leaveRequest, IWebHostEnvironment environment)
        {
            _leaveRequest = leaveRequest;
            _environment = environment;
        }

        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee Supervisor, Employee")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Leaverequest leaveRequest, IFormFile? file)
        {
            if (leaveRequest == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            try
            {
                var addLeaveRequest = await _leaveRequest.AddLeaveRequestAsync(leaveRequest, file);

                return Ok(new { status = "Success!", data = addLeaveRequest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Employee Supervisor, HR Manager")]
        [HttpPut("approval/{processId}")]
        public async Task<IActionResult> ApprovalAsync(int processId, [FromBody] Process process)
        {
            if (process == null)
            {
                return BadRequest(new { message = "Invalid request data." });
            }

            try
            {
                await _leaveRequest.ApprovalAsync(processId, process);
                return Ok(new { status = "Success!", message = "Leave request processed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new { message = "An internal error occurred." });
            }
        }

        [Authorize(Roles = "Administrator, HR Manager, Employee Supervisor, Department Manager, Employee")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeaveRequest()
        {
            var requests = await _leaveRequest.GetAllLeaveRequestsAsync();

            return Ok(new { status = "Success!", data = requests });
        }

        [Authorize]
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllLeaveRequestPaging([FromQuery] QueryObjectLeaveRequest query)
        {
            var requests = await _leaveRequest.GetAllLeaveRequestsPagingAsync(query);

            return Ok(requests);
        }

        [Authorize(Roles = "Administrator, HR Manager, Employee Supervisor, Department Manager, Employee")]
        [HttpGet("{requestId}")]
        public async Task<IActionResult> GetLeaveRequestById(int requestId)
        {
            try
            {
                var requests = await _leaveRequest.GetLeaveRequestByIdAsync(requestId);
                if (requests == null)
                {
                    return NotFound(new { message = $"Leave request with ID {requestId} not found." });
                }
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
