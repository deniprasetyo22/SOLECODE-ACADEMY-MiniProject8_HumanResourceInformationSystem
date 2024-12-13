using Microsoft.AspNetCore.Http;
using MiniProject8.Application.DTOs;
using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Interfaces.IServices
{
    public interface ILeaveRequestService
    {
        Task<Leaverequest> AddLeaveRequestAsync(Leaverequest leaveRequest, IFormFile? file);
        Task ApprovalAsync(int processId, Process process);
        Task<object> GetAllLeaveRequestsAsync();
        Task<object> GetLeaveRequestByIdAsync(int id);
        Task<object> GetAllLeaveRequestsPagingAsync(QueryObjectLeaveRequest query);
    }
}
