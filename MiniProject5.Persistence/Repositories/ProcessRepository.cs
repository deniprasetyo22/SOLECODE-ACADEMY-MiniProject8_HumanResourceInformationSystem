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
    public class ProcessRepository : IProcessRepository
    {
        private readonly HrisContext _context;
        public ProcessRepository(HrisContext context)
        {
            _context = context;
        }

        public async Task<Process> GetProcessByIdAsync(int processId)
        {
            return await _context.Processes
                .Include(b => b.Requester)
                .FirstOrDefaultAsync(b => b.Processid == processId);

        }

        public async Task<IEnumerable<Process>> GetAllProcessAsync()
        {
            return await _context.Processes
                .Include(b => b.Requester)
                .Include(b => b.Workflow)
                .Include(b => b.Leaverequests)
                .ToListAsync();
        }

        public async Task AddProcessAsync(Process process)
        {
            await _context.Processes.AddAsync(process);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProcessAsync(Process process)
        {
            _context.Processes.Update(process);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProcessAsync(int processId)
        {
            var existingProcess = await _context.Processes.FindAsync(processId);
            if (existingProcess != null)
            {
                _context.Processes.Remove(existingProcess);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Process>> GetProcessesByRequesterIdAsync(string requesterId)
        {
            return await _context.Processes
                .Include(p => p.Requester)
                .Include(p => p.Workflow)
                .Include(p => p.Leaverequests)
                .Where(p => p.Requesterid == requesterId)
                .ToListAsync();
        }
    }
}
