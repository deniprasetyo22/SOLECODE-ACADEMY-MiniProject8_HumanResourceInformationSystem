using MiniProject8.Application.DTOs;
using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Interfaces.IServices
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsNoPagesAsync();
        Task<object> GetAllProjectsAsync(QueryObjectProject query);
        Task<Project> GetProjectByIdAsync(int projId);
        Task<Project> AddProjectAsync(Project project);
        Task UpdateProjectAsync(int projId, Project project);
        Task DeleteProjectAsync(int projId);
    }
}
