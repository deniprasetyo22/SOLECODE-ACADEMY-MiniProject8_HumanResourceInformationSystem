using MiniProject5.Application.DTOs;
using MiniProject5.Persistence.Models;
using MiniProject8.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Interfaces.IRepositories
{
    public interface IWorksOnRepository
    {
        Task<IEnumerable<Workson>> GetAllWorksOnAsync(paginationDto pagination);
        Task<Workson> GetWorksOnByIdAsync(int empId, int projId);
        Task<Workson> AddWorksOnAsync(Workson worksOn);
        Task UpdateWorksOnAsync(int empId, int projId, Workson worksOn);
        Task DeleteWorksOnAsync(int empId, int projId);
        Task<Workson> GetOwnWorkson();

        //Top 5 employees by performance
        Task<List<TopEmployeeDto>> GetTopEmployeesByPerformanceAsync();
    }
}
