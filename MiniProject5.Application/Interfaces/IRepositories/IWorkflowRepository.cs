using Microsoft.EntityFrameworkCore;
using MiniProject6.Persistence.Models;
using MiniProject8.Application.DTOs;
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
        Task ApproveOrRejectLeaveRequestAsync(int processId, Process Process);
        Task<List<ProcessDto>> GetAllProcessesAsync();
    }
}
