using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject8.Application.Interfaces.IServices;

namespace MiniProject8.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IEmployeeRepository _employeeService;
        public DashboardController(IDashboardService dashboardService, IEmployeeRepository employeeRepository)
        {
            _dashboardService = dashboardService;
            _employeeService = employeeRepository;
        }

        [HttpGet("employee-distribution-by-department")]
        public async Task<IActionResult> GetEmployeePercentageByDepartment()
        {
            var result = await _dashboardService.GetEmployeePercentageByDepartmentAsync();
            return Ok(result);
        }

        [HttpGet("average-salary-by-department")]
        public async Task<IActionResult> GetAverageSalaryByDepartment()
        {
            var result = await _dashboardService.GetAverageSalaryByDepartmentAsync();
            return Ok(result);
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees([FromQuery] int? departmentId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var employees = await _employeeService.GetEmployeesByDepartmentAsync(departmentId, pageNumber, pageSize);
            var totalCount = await _employeeService.GetTotalCountByDepartmentAsync(departmentId);

            return Ok(new
            {
                TotalCount = totalCount,
                Employees = employees
            });
        }

        [HttpGet("employee-report-pdf")]
        public async Task<IActionResult> GenerateEmployeeReportPdf([FromQuery] int? departmentId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var pdfBytes = await _dashboardService.GenerateEmployeeReportPdfAsync(departmentId, pageNumber, pageSize);
            return File(pdfBytes, "application/pdf", "EmployeeReport.pdf");
        }
    }
}
