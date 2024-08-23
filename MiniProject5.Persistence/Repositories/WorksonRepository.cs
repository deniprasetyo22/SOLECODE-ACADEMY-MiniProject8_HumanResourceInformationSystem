using Microsoft.EntityFrameworkCore;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Persistence.Context;
using MiniProject5.Persistence.Models;
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

        public WorksOnRepository(HrisContext context)
        {
            _context = context;
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
    }
}
