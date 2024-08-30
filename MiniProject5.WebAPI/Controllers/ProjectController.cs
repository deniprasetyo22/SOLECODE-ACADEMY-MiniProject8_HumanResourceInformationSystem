using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Persistence.Models;

namespace MiniProject5.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize(Roles = "Administrator, HR Manager, Department Manager")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects([FromQuery] paginationDto pagination)
        {
            var projects = await _projectService.GetAllProjectsAsync(pagination);
            return Ok(projects);
        }

        [Authorize(Roles = "Administrator, HR Manager, Department Manager, Employee")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [Authorize(Roles = "Administrator, Department Manager")]
        [HttpPost]
        public async Task<ActionResult<Project>> AddProject(Project project)
        {
            var newProject = await _projectService.AddProjectAsync(project);
            return Ok(newProject);
        }

        [Authorize(Roles = "Administrator, Department Manager")]
        [HttpPut("{projId}")]
        public async Task<IActionResult> UpdateProject(int projId,[FromBody] Project project)
        {
            await _projectService.UpdateProjectAsync(projId, project);
            return Ok();
        }

        [Authorize(Roles = "Administrator, Department Manager")]
        [HttpDelete("{projId}")]
        public async Task<IActionResult> DeleteProject(int projId)
        {
            await _projectService.DeleteProjectAsync(projId);
            return Ok();
        }
    }
}
