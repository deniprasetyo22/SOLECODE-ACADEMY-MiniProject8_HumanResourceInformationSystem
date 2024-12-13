using Microsoft.EntityFrameworkCore;
using MiniProject8.Application.Interfaces.IRepositories;
using MiniProject8.Domain.Models;
using MiniProject8.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Persistence.Repositories
{
    public class WorksOnRepository : IWorksOnRepository
    {
        private readonly HrisContext _context;

        public WorksOnRepository(HrisContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Workson>> GetAllWorksOnAsync()
        {
            return await _context.Worksons
                .Include(b=>b.Emp)
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

        public async Task UpdateWorksOnAsync(Workson worksOn)
        {
            _context.Worksons.Update(worksOn);
            await _context.SaveChangesAsync();
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
