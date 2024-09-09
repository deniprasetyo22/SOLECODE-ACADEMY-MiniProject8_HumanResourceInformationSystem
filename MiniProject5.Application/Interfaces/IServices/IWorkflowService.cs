using MiniProject6.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Application.Interfaces.IServices
{
    public interface IWorkflowService
    {
        Task<Leaverequest> SubmitLeaveRequestAsync(Leaverequest request);
        Task ApproveOrRejectLeaveRequestAsync(int processId, Process Process);
    }
}
