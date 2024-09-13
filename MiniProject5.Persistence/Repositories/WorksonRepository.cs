using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Persistence.Context;
using MiniProject5.Persistence.Models;
using MiniProject6.Domain.Models;
using MiniProject8.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Persistence.Repositories
{
    public class WorksOnRepository : IWorksOnRepository
    {
        private readonly HrisContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public WorksOnRepository(HrisContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Workson>> GetAllWorksOnAsync(paginationDto pagination)
        {
            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;
            return await _context.Worksons
                .Include(w => w.Emp)
                .Include(w => w.Proj)
                .Skip(skipNumber)
                .Take(pagination.pageSize)
                .ToListAsync();
        }

        public async Task<Workson> GetWorksOnByIdAsync(int empId, int projId)
        {
            return await _context.Worksons.FindAsync(empId, projId);
        }

        public async Task<Workson> AddWorksOnAsync(Workson worksOn)
        {
            _context.Worksons.Add(worksOn);
            await _context.SaveChangesAsync();
            return worksOn;
        }

        public async Task UpdateWorksOnAsync(int empId, int projId, Workson worksOn)
        {
            var existingWorkson = await _context.Worksons.FirstOrDefaultAsync(w => w.Empid == empId && w.Projid == projId);
            if (existingWorkson != null)
            {
                existingWorkson.Dateworked = worksOn.Dateworked;
                existingWorkson.Hoursworked = worksOn.Hoursworked;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteWorksOnAsync(int empId, int projId)
        {
            var worksOn = await _context.Worksons.FindAsync(empId, projId);
            if (worksOn != null)
            {
                _context.Worksons.Remove(worksOn);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Workson> GetOwnWorkson()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);

            var userId = currentUser?.Id;

            if(string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User ID not found in context");
                return null;
            }

            var ownWorkson = await _context.Worksons
                .Include(w => w.Proj)
                .FirstOrDefaultAsync(e => e.Userid == userId);

            if (ownWorkson == null)
            {
                Console.WriteLine("Workson not found");
                return null;
            }

            return ownWorkson;
        }

        //Top 5 employees by performance 
        public async Task<List<TopEmployeeDto>> GetTopEmployeesByPerformanceAsync()
        {
            return await _context.Worksons
                .Where(w => w.Hoursworked.HasValue) // Memastikan hoursworked tidak null
                .GroupBy(w => w.Empid)
                .Select(g => new TopEmployeeDto
                {
                    EmployeeName = _context.Employees
                        .Where(e => e.Empid == g.Key)
                        .Select(e => $"{e.Fname} {e.Lname}")
                        .FirstOrDefault() ?? "Unknown",
                    TotalHoursWorked = g.Sum(w => w.Hoursworked.Value)
                })
                .OrderByDescending(e => e.TotalHoursWorked)
                .Take(5)
                .ToListAsync();
        }
    }
}
