using Microsoft.EntityFrameworkCore;
using MiniProject6.Application.Interfaces.IRepositories;
using MiniProject6.Application.Interfaces.IServices;
using MiniProject6.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Application.Services
{
    public class WorkflowService:IWorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;
        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        public async Task<Leaverequest> SubmitLeaveRequestAsync(Leaverequest request)
        {
            return await _workflowRepository.SubmitLeaveRequestAsync(request);
        }

        public async Task<Workflowaction> AddActionAsync(Workflowaction workflowAction)
        {
            return await _workflowRepository.AddActionAsync(workflowAction);
        }

        public async Task ApproveOrRejectLeaveRequestBySupervisorAsync(int processId, Process Process)
        {
            await _workflowRepository.ApproveOrRejectLeaveRequestBySupervisorAsync(processId, Process);
        }

        public async Task ApproveOrRejectLeaveRequestByHRAsync(int processId, Process Process)
        {
            await _workflowRepository.ApproveOrRejectLeaveRequestByHRAsync(processId, Process);
        }
    }
}
