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
    public class ProjectRepository : IProjectRepository
    {
        private readonly HrisContext _context;

        public ProjectRepository(HrisContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(paginationDto pagination)
        {
            var skipNumber = (pagination.pageNumber - 1) * pagination.pageSize;
            return await _context.Projects
                .Skip(skipNumber)
                .Take(pagination.pageSize)
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

        public async Task UpdateProjectAsync(int projId, Project project)
        {
            var existingProj = await _context.Projects.FirstOrDefaultAsync(cek => cek.Projid == projId);
            if (existingProj != null)
            {
                existingProj.Projname = project.Projname;
                existingProj.Deptid = project.Deptid;
                await _context.SaveChangesAsync();
            }
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
