using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Persistence.Models;

namespace MiniProject5.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects([FromQuery] paginationDto pagination)
        {
            var projects = await _projectService.GetAllProjectsAsync(pagination);
            return Ok(projects);
        }

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

        [HttpPost]
        public async Task<ActionResult<Project>> AddProject(Project project)
        {
            var newProject = await _projectService.AddProjectAsync(project);
            return Ok(newProject);
        }

        [HttpPut("{projId}")]
        public async Task<IActionResult> UpdateProject(int projId,[FromBody] Project project)
        {
            await _projectService.UpdateProjectAsync(projId, project);
            return Ok();
        }

        [HttpDelete("{projId}")]
        public async Task<IActionResult> DeleteProject(int projId)
        {
            await _projectService.DeleteProjectAsync(projId);
            return Ok();
        }
    }
}
