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
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly HrisContext _context;

        public WorkflowRepository(HrisContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Workflow>> GetAllWorkflowsAsync()
        {
            return await _context.Workflows.ToListAsync();
        }

        public async Task<Workflow> GetWorkflowByIdAsync(int workflowId)
        {
            return await _context.Workflows.FindAsync(workflowId);
        }

        public async Task AddWorkflowAsync(Workflow workflow)
        {
            await _context.Workflows.AddAsync(workflow);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWorkflowAsync(Workflow workflow)
        {
            _context.Workflows.Update(workflow);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkflowAsync(int workflowId)
        {
            var existingWorkflow = await _context.Workflows.FindAsync(workflowId);
            if (existingWorkflow != null)
            {
                _context.Workflows.Remove(existingWorkflow);
                await _context.SaveChangesAsync();
            }
        }
    }
}
