using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Persistence.Models;

namespace MiniProject5.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksOnController : ControllerBase
    {
        private readonly IWorksOnService _worksOnService;

        public WorksOnController(IWorksOnService worksOnService)
        {
            _worksOnService = worksOnService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workson>>> GetAllWorksOn([FromQuery] paginationDto pagination)
        {
            var worksOnList = await _worksOnService.GetAllWorksOnAsync(pagination);
            return Ok(worksOnList);
        }

        [HttpGet("{empId}/{projId}")]
        public async Task<ActionResult<Workson>> GetWorksOn(int empId, int projId)
        {
            var worksOn = await _worksOnService.GetWorksOnByIdAsync(empId, projId);
            if (worksOn == null)
            {
                return NotFound();
            }
            return Ok(worksOn);
        }

        [HttpPost]
        public async Task<ActionResult<Workson>> AddWorksOn(Workson worksOn)
        {
            var newWorksOn = await _worksOnService.AddWorksOnAsync(worksOn);
            return Ok(newWorksOn);
        }

        [HttpPut("{empId}/{projId}")]
        public async Task<IActionResult> UpdateWorksOn(int empId, int projId, [FromBody] Workson worksOn)
        {
            await _worksOnService.UpdateWorksOnAsync(empId, projId, worksOn);
            return Ok();
        }

        [HttpDelete("{empId}/{projId}")]
        public async Task<IActionResult> DeleteWorksOn(int empId, int projId)
        {
            await _worksOnService.DeleteWorksOnAsync(empId, projId);
            return Ok();
        }
    }
}
