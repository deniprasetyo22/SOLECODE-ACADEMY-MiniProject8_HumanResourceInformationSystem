using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject8.Application.Interfaces.IServices;
using PdfSharpCore;

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

        //Employee Distribution By Department
        [HttpGet("employee-distribution-by-department")]
        public async Task<IActionResult> GetEmployeePercentageByDepartment()
        {
            var result = await _dashboardService.GetEmployeePercentageByDepartmentAsync();
            return Ok(result);
        }

        //Top 5 employees by performance
        [HttpGet("top-employees-by-performance")]
        public async Task<IActionResult> GetTopEmployeesByPerformance()
        {
            var topEmployees = await _dashboardService.GetTopEmployeesByPerformanceAsync();
            return Ok(topEmployees);
        }

        //Average salary by department
        [HttpGet("average-salary-by-department")]
        public async Task<IActionResult> GetAverageSalaryByDepartment()
        {
            var result = await _dashboardService.GetAverageSalaryByDepartmentAsync();
            return Ok(result);
        }

        //Workflow Process
        [Authorize(Roles = "HR Manager, Employee Supervisor")]
        [HttpGet("workflow-processes")]
        public async Task<IActionResult> GetAllProcesses()
        {
            try
            {
                var processes = await _dashboardService.GetAllProcessesAsync();
                return Ok(processes);
            }
            catch (Exception ex)
            {
                // Tangani kesalahan
                return StatusCode(500, $"Terjadi kesalahan: {ex.Message}");
            }
        }

        //Employee Leaves
        [HttpGet("employee-leaves")]
        public async Task<IActionResult> GetEmployeeLeave(DateTime startDate, DateTime endDate)
        {
            try
            {
                var employeeLeave = await _dashboardService.GetEmployeeLeavesAsync(startDate, endDate);
                return Ok(employeeLeave);
            }
            catch (Exception ex)
            {
                // Tangani kesalahan
                return StatusCode(500, $"Terjadi kesalahan: {ex.Message}");
            }
        }

        //List Employee By Department
        [HttpGet("list-employee-by-department")]
        public async Task<IActionResult> GetListEmployeeByDepartment([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string departmentName)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var listEmployee = await _dashboardService.GetListEmployeeByDepartment(pageNumber, pageSize, departmentName);
            
            return Ok(listEmployee);
        }
        
        //Report List Employee By Department
        [HttpGet("report-list-employee-by-department")]
        public async Task<IActionResult> GetReportListEmployeeByDepartment([FromQuery] string departmentName)
        {
            var listEmployee = await _dashboardService.GetReportListEmployeeByDepartment(departmentName);
            
            return File(listEmployee, "application/pdf", "ListEmployeeByDepartment.pdf");
        }

        //Report Employee Leaves 
        [HttpGet("report-employee-leaves")]
        public async Task<IActionResult> GetReportEmployeeLeavesAsync(DateTime startDate, DateTime endDate)
        {
            var pdfBytes = await _dashboardService.GetReportEmployeeLeavesAsync(startDate, endDate);

            return File(pdfBytes, "application/pdf", "EmployeeLeavesReport.pdf");
        }
    }
}
