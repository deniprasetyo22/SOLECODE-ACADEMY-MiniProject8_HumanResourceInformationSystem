using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject8.Application.Interfaces.IServices;
using MiniProject8.Application.Services;
using PdfSharpCore;

namespace MiniProject8.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeDistributionByDepartment()
        {
            try
            {
                var empDistribution = await _dashboardService.GetEmployeeDistributionByDepartmentAsync();
                var topEmployees = await _dashboardService.GetTopEmployeesByPerformanceAsync();
                var averageSalary = await _dashboardService.GetAverageSalaryByDepartmentAsync();
                var processes = await _dashboardService.GetWorkflowProcessesAsync();
                var employeesTotal = await _dashboardService.GetEmployeesTotalAsync();
                var departmentsTotal = await _dashboardService.GetDepartmentsTotalAsync();
                var projectsTotal = await _dashboardService.GetProjectsTotalAsync();
                var assignmentsTotal = await _dashboardService.GetAssignmentsTotalAsync();
                return Ok(new
                {
                    EmployeeDistribution = empDistribution,
                    TopEmployees = topEmployees,
                    AverageSalary = averageSalary,
                    Processes = processes,
                    EmployeesTotal = employeesTotal,
                    DepartmentsTotal = departmentsTotal,
                    AssignmentsTotal = assignmentsTotal,
                    ProjectsTotal = projectsTotal,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("list-employees-by-department-report")]
        public async Task<IActionResult> GetListEmployeeByDepartmentReportAsync([FromQuery] string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                return BadRequest(new {message = "Department name is required." });
            }

            try
            {
                var pdfBytes = await _dashboardService.GetListEmployeeByDepartmentReportAsync(departmentName);
                return File(pdfBytes, "application/pdf", $"{departmentName}_Employee_Report.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }

        [Authorize]
        [HttpGet("employee-leaves-report")]
        public async Task<IActionResult> GetEmployeeLeavesReportAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var pdfBytes = await _dashboardService.GetEmployeeLeavesReportAsync(startDate, endDate);
                return File(pdfBytes, "application/pdf", $"Employee_Leaves_Report_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("projects-report")]
        public async Task<IActionResult> GetAllProjectsReport()
        {
            try
            {
                var reportBytes = await _dashboardService.GetAllProjectsReportAsync();
                return File(reportBytes, "application/pdf", "ProjectsReport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
