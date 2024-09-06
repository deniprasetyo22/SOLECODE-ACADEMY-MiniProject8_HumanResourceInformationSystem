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

        [HttpPost("add-action")]
        public async Task<ActionResult<Workflowaction>> AddAction(Workflowaction workflowAction)
        {
            var result = await _workflowService.AddActionAsync(workflowAction);
            return Ok(result);
        }

        [Authorize(Roles = "Employee Supervisor")]
        [HttpPut("approveOrRejectBySupervisor/{processId}")]
        public async Task<IActionResult> approveOrRejectLeaveRequestBySupervisorAsync(int processId, [FromBody] Process process)
        {
            await _workflowService.ApproveOrRejectLeaveRequestBySupervisorAsync(processId, process);
            return Ok();
        }

        [Authorize(Roles = "HR Manager")]
        [HttpPut("approveOrRejectByHR/{processId}")]
        public async Task<IActionResult> approveOrRejectLeaveRequestByHRAsync(int processId, [FromBody] Process process)
        {
            await _workflowService.ApproveOrRejectLeaveRequestByHRAsync(processId, process);
            return Ok();
        }
    }
}
