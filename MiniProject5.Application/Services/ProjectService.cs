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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(paginationDto pagination)
        {
            return await _projectRepository.GetAllProjectsAsync(pagination);
        }

        public async Task<Project> GetProjectByIdAsync(int projId)
        {
            return await _projectRepository.GetProjectByIdAsync(projId);
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            return await _projectRepository.AddProjectAsync(project);
        }

        public async Task UpdateProjectAsync(int projId, Project project)
        {
            await _projectRepository.UpdateProjectAsync(projId, project);
        }

        public async Task DeleteProjectAsync(int projId)
        {
            await _projectRepository.DeleteProjectAsync(projId);
        }
    }
}
