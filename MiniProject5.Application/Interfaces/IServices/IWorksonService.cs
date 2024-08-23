using MiniProject5.Application.DTOs;
using MiniProject5.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Interfaces.IServices
{
    public interface IWorksOnService
    {
        Task<IEnumerable<Workson>> GetAllWorksOnAsync(paginationDto pagination);
        Task<Workson> GetWorksOnByIdAsync(int empId, int projId);
        Task<Workson> AddWorksOnAsync(Workson worksOn);
        Task UpdateWorksOnAsync(int empId, int projId, Workson worksOn);
        Task DeleteWorksOnAsync(int empId, int projId);
    }
}
