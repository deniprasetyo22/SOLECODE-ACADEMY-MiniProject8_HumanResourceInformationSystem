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
    public class ProjectRepository : IProjectRepository
    {
        private readonly HrisContext _context;

        public ProjectRepository(HrisContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(b=>b.Dept)
                .Include(b=>b.Worksons).ThenInclude(b=>b.Emp)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int projId)
        {
            return await _context.Projects.FindAsync(projId);
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int projId)
        {
            var project = await _context.Projects.FindAsync(projId);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}
