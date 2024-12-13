using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MiniProject8.Application.DTOs;
using MiniProject8.Application.Interfaces.IRepositories;
using MiniProject8.Application.Interfaces.IServices;
using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Services
{
    public class WorksOnService : IWorksOnService
    {
        private readonly IWorksOnRepository _worksOnRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public WorksOnService(IWorksOnRepository worksOnRepository, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _worksOnRepository = worksOnRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<object> GetAllWorksOnAsync(paginationDto pagination)
        {
            var worksOn = await _worksOnRepository.GetAllWorksOnAsync();
            var total = worksOn.Count();

            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;

            var worksOnList = worksOn.Skip(skipNumber).Take(pagination.pageSize).ToList();

            return new { total = total, data = worksOnList };
        }

        public async Task<IEnumerable<Workson>> GetAllWorksOnNoPagesAsync()
        {
            return await _worksOnRepository.GetAllWorksOnAsync();
        }

        public async Task<Workson> GetWorksOnByIdAsync(int empId, int projId)
        {
            var worksOn = await _worksOnRepository.GetWorksOnByIdAsync(empId, projId);
            if (worksOn == null)
            {
                throw new KeyNotFoundException($"Work record not found for Employee ID {empId} and Project ID {projId}.");
            }
            return worksOn;
        }

        public async Task<Workson> AddWorksOnAsync(int empId, int projId, Workson worksOn)
        {
            var existingWorkson = await _worksOnRepository.GetWorksOnByIdAsync(empId, projId);
            if (existingWorkson != null && existingWorkson.Empid == empId && existingWorkson.Projid == projId)
            {
                throw new Exception($"Employee with ID {empId} is already assigned to project with ID {projId}");
            }
            return await _worksOnRepository.AddWorksOnAsync(worksOn);
        }

        public async Task UpdateWorksOnAsync(int empId, int projId, Workson worksOn)
        {
            var existingWorkson = await _worksOnRepository.GetWorksOnByIdAsync(empId, projId);
            if (existingWorkson == null)
            {
                throw new KeyNotFoundException($"Work record not found for Employee ID {empId} and Project ID {projId}.");
            }

            existingWorkson.Dateworked = worksOn.Dateworked;
            existingWorkson.Hoursworked = worksOn.Hoursworked;

            await _worksOnRepository.UpdateWorksOnAsync(existingWorkson);
        }

        public async Task DeleteWorksOnAsync(int empId, int projId)
        {
            var existingWorksOn = await _worksOnRepository.GetWorksOnByIdAsync(empId, projId);
            if (existingWorksOn == null)
            {
                throw new KeyNotFoundException($"Work record not found for Employee ID {empId} and Project ID {projId}.");
            }

            await _worksOnRepository.DeleteWorksOnAsync(empId, projId);
        }

        public async Task<Workson> GetOwnWorksonAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                throw new InvalidOperationException("User name not found in HttpContext.");
            }

            var currentUser = await _userManager.FindByNameAsync(userName);
            var userId = currentUser?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User ID not found in context.");
            }

            // Retrieve all works on records and filter by user ID
            var worksOnRecords = await _worksOnRepository.GetAllWorksOnAsync();
            var ownWorkson = worksOnRecords.FirstOrDefault(w => w.Userid == userId);

            if (ownWorkson == null)
            {
                throw new KeyNotFoundException("Workson not found for the current user.");
            }

            return ownWorkson;
        }
    }
}
