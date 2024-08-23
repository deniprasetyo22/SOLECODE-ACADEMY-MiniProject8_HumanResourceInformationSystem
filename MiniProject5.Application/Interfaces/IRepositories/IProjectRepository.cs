using MiniProject5.Application.DTOs;
using MiniProject5.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Interfaces.IRepositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync(paginationDto pagination);
        Task<Project> GetProjectByIdAsync(int projId);
        Task<Project> AddProjectAsync(Project project);
        Task UpdateProjectAsync(int projId, Project project);
        Task DeleteProjectAsync(int projId);
    }
}
