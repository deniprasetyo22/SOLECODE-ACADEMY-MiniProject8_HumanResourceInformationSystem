using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Interfaces.IRepositories
{
    public interface IWorkflowRepository
    {
        //Task<Leaverequest> SubmitLeaveRequestAsync(Leaverequest request);
        //Task ApprovalLeaveRequest(int processId, Process Process);

        Task<IEnumerable<Workflow>> GetAllWorkflowsAsync();
        Task<Workflow> GetWorkflowByIdAsync(int workflowId);
        Task AddWorkflowAsync(Workflow workflow);
        Task UpdateWorkflowAsync(Workflow workflow);
        Task DeleteWorkflowAsync(int workflowId);
    }
}
