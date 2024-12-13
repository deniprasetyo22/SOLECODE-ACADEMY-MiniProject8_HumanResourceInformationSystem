using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Interfaces.IRepositories
{
    public interface ILeaveRequestRepository
    {
        Task<Leaverequest> AddLeaveRequestAsync(Leaverequest leaveRequest);
        IQueryable<Leaverequest> GetAllLeaveRequestsAsync();
        Task<Leaverequest> GetLeaveRequestByIdAsync(int requestId);
        Task<IEnumerable<Leaverequest>> GetLeaveRequestsByUserIdAsync(string userId);
        Task<IEnumerable<Leaverequest>> GetAllLeaveRequestsListAsync();
    }
}
