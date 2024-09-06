using Microsoft.EntityFrameworkCore;
using MiniProject6.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Application.Interfaces.IRepositories
{
    public interface IWorkflowRepository
    {
        Task<Leaverequest> SubmitLeaveRequestAsync(Leaverequest request);
        Task<Workflowaction> AddActionAsync(Workflowaction workflowAction);
        Task ApproveOrRejectLeaveRequestBySupervisorAsync(int processId, Process Process);
        Task ApproveOrRejectLeaveRequestByHRAsync(int processId, Process Process);
    }
}
