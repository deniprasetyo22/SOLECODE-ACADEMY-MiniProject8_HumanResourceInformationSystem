using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Services
{
    public class WorksOnService : IWorksOnService
    {
        private readonly IWorksOnRepository _worksOnRepository;

        public WorksOnService(IWorksOnRepository worksOnRepository)
        {
            _worksOnRepository = worksOnRepository;
        }

        public async Task<IEnumerable<Workson>> GetAllWorksOnAsync(paginationDto pagination)
        {
            return await _worksOnRepository.GetAllWorksOnAsync(pagination);
        }

        public async Task<Workson> GetWorksOnByIdAsync(int empId, int projId)
        {
            return await _worksOnRepository.GetWorksOnByIdAsync(empId, projId);
        }

        public async Task<Workson> AddWorksOnAsync(Workson worksOn)
        {
            return await _worksOnRepository.AddWorksOnAsync(worksOn);
        }

        public async Task UpdateWorksOnAsync(int empId, int projId, Workson worksOn)
        {
            await _worksOnRepository.UpdateWorksOnAsync(empId, projId, worksOn);
        }

        public async Task DeleteWorksOnAsync(int empId, int projId)
        {
            await _worksOnRepository.DeleteWorksOnAsync(empId, projId);
        }

        public async Task<Workson> GetOwnWorkson()
        {
            return await _worksOnRepository.GetOwnWorkson();
        }
    }
}
