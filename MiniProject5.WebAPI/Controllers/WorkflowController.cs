using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject6.Application.Interfaces.IServices;
using MiniProject6.Persistence.Models;

namespace MiniProject6.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;
        public WorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost("add-leaverequest")]
        public async Task<ActionResult<Leaverequest>> AddProcessLeaveRequest(Leaverequest request)
        {
            var result = await _workflowService.SubmitLeaveRequestAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "HR Manager, Employee Supervisor")]
        [HttpPut("approveOrReject/{processId}")]
        public async Task<IActionResult> approveOrRejectLeaveRequestAsync(int processId, [FromBody] Process process)
        {
            await _workflowService.ApproveOrRejectLeaveRequestAsync(processId, process);
            return Ok();
        }
    }
}
