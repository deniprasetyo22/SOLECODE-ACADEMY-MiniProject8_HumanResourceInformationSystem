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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<IEnumerable<Project>> GetAllProjectsNoPagesAsync()
        {
            return await _projectRepository.GetAllProjectsAsync();
        }

        public async Task<object> GetAllProjectsAsync(QueryObjectProject query)
        {
            var project = await _projectRepository.GetAllProjectsAsync();
            var temp = project.AsQueryable();

            if (!string.IsNullOrEmpty(query.Keyword))
            {
                string keywordLower = query.Keyword.ToLower();
                temp = temp.Where(b => b.Projname.ToLower().Contains(keywordLower));
            }

            var total = temp.Count();

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "projname":
                        temp = query.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                            ? temp.OrderBy(s => s.Projname)
                            : temp.OrderByDescending(s => s.Projname);
                        break;
                    default:
                        temp = query.SortOrder.Equals("asc")
                            ? temp.OrderBy(s => s.Projid)
                            : temp.OrderByDescending(s => s.Projid);
                        break;
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            var projList = temp.Skip(skipNumber).Take(query.PageSize).ToList();

            return new { total = total, data = projList };
        }


        public async Task<Project> GetProjectByIdAsync(int projId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projId);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {projId} not found.");
            }
            return project;
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            var existingProj = await _projectRepository.GetAllProjectsAsync();

            if (existingProj.Any(e => e.Projname == project.Projname))
            {
                throw new InvalidOperationException("Project with the same name already exists.");
            }

            return await _projectRepository.AddProjectAsync(project);
        }

        public async Task UpdateProjectAsync(int projId, Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            var existingProj = await _projectRepository.GetProjectByIdAsync(projId);
            if (existingProj == null)
            {
                throw new KeyNotFoundException($"Project with ID {projId} not found.");
            }

            var allProjects = await _projectRepository.GetAllProjectsAsync();

            // Check for SSN conflict
            if (allProjects.Any(e => e.Projname == project.Projname && e.Projid != projId))
            {
                throw new InvalidOperationException("Project with the same name already exists.");
            }

            // Update properties
            existingProj.Projname = project.Projname;
            existingProj.Deptid = project.Deptid;

            await _projectRepository.UpdateProjectAsync(existingProj);
        }

        public async Task DeleteProjectAsync(int projId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projId);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {projId} not found.");
            }

            await _projectRepository.DeleteProjectAsync(projId);
        }
    }
}
