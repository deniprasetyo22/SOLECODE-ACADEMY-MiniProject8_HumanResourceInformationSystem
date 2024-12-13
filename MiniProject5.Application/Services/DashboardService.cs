using MiniProject8.Application.DTOs;
using MiniProject8.Application.Interfaces.IServices;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Reflection.Metadata;
using System.Xml.Linq;
using MiniProject8.Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MiniProject8.Domain.Models;
using static MiniProject8.Application.DTOs.LeaveRequestDto;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MiniProject8.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorksOnRepository _worksOnRepository;
        private readonly IProcessRepository _processRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<AppUser> _userManager;

        public DashboardService(IEmployeeRepository employeeRepository, 
            IWorksOnRepository worksOnRepository, 
            IProcessRepository processRepository, 
            ILeaveRequestRepository leaveRequestRepository, 
            IHttpContextAccessor httpContextAccessor, 
            UserManager<AppUser> userManager,
            IDepartmentRepository departmentRepository,
            IProjectRepository projectRepository)
        {
            _employeeRepository = employeeRepository;
            _worksOnRepository = worksOnRepository;
            _processRepository = processRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _departmentRepository = departmentRepository;
            _projectRepository = projectRepository;
        }

        public async Task<List<EmployeeDistributionByDeptDto>> GetEmployeeDistributionByDepartmentAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var distribution = employees
                .GroupBy(w => w.Deptid)
                .Select(g => new EmployeeDistributionByDeptDto
                {
                    DeptId = g.Key,
                    DeptName = g.Select(e => e.Dept.Deptname).FirstOrDefault(),
                    EmployeeCount = g.Count()
                })
                .ToList();

            return distribution;
        }

        public async Task<List<TopEmployeeDto>> GetTopEmployeesByPerformanceAsync()
        {
            var workRecords = await _worksOnRepository.GetAllWorksOnAsync();

            var topEmployees = workRecords
                .GroupBy(w => w.Empid)
                .Select(g => new TopEmployeeDto
                {
                    EmpId = g.Key,
                    EmployeeName = $"{g.Select(w => w.Emp.Fname).FirstOrDefault()} {g.Select(w => w.Emp.Lname).FirstOrDefault()}",
                    TotalHoursWorked = g.Sum(w => w.Hoursworked) ?? 0
                })
                .OrderByDescending(e => e.TotalHoursWorked)
                .Take(5)
                .ToList();

            return topEmployees;
        }

        public async Task<List<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            
            var averageSalaries = employees
                .GroupBy(w => w.Deptid)
                .Select(g => new AverageSalaryDto
                {
                    DeptId = g.Key,
                    DeptName = g.Select(e => e.Dept.Deptname).FirstOrDefault(),
                    AverageSalary = g.Average(e => e.Salary)
                })
                .ToList();

            return averageSalaries;
        }

        public async Task<object> GetWorkflowProcessesAsync()
        {
            var userRoles = _httpContextAccessor.HttpContext?.User.Claims
                 .Where(c => c.Type == ClaimTypes.Role)
                 .Select(c => c.Value)
                 .ToList();

            var user = _httpContextAccessor.HttpContext?.User.Identity!.Name;

            var currentUser = await _userManager.FindByNameAsync(user!);
            if (currentUser == null)
            {
                throw new InvalidOperationException("Current user not found.");
            }

            if (userRoles.Contains("Employee Supervisor"))
            {
                var leaveRequests = await _leaveRequestRepository.GetAllLeaveRequestsListAsync();
                var reviewedRequests = leaveRequests
                    .Where(b => b.Process.Status == "Reviewed By Supervisor")
                    .Select(b => new
                    {
                        RequestId = b.Requestid,
                        RequestName = b.Requestname,
                        StartDate = b.Startdate,
                        EndDate = b.Enddate,
                        TotalDays = b.Totaldays,
                        LeaveType = b.Leavetype,
                        Reason = b.Reason,
                        Requester = b.Emp.Fname + ' ' + b.Emp.Lname,
                        Status = b.Process.Status,
                        File = b.FileUrl,
                    })
                    .ToList();
                return reviewedRequests;
            }

            if (userRoles.Contains("HR Manager"))
            {
                var leaveRequests = await _leaveRequestRepository.GetAllLeaveRequestsListAsync();
                var reviewedRequests = leaveRequests
                    .Where(b => b.Process.Status == "Reviewed By HR Manager")
                    .Select(b => new
                    {
                        RequestId = b.Requestid,
                        RequestName = b.Requestname,
                        StartDate = b.Startdate,
                        EndDate = b.Enddate,
                        TotalDays = b.Totaldays,
                        LeaveType = b.Leavetype,
                        Reason = b.Reason,
                        Requester = b.Emp.Fname + ' ' + b.Emp.Lname,
                        Status = b.Process.Status,
                        File = b.FileUrl,
                    })
                    .ToList();
                return reviewedRequests;
            }

            return new List<object>();
        }

        public async Task<object> GetEmployeesTotalAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            var total = employees.Count();
            return new { EmployeesTotal = total };
        }

        public async Task<object> GetDepartmentsTotalAsync()
        {
            var departments = await _departmentRepository.GetAllDepartmentsAsync();
            var total = departments.Count();
            return new { DepartmentsTotal = total };
        }

        public async Task<object> GetProjectsTotalAsync()
        {
            var projects = await _projectRepository.GetAllProjectsAsync();
            var total = projects.Count();
            return new { ProjectsTotal = total };
        }

        public async Task<object> GetAssignmentsTotalAsync()
        {
            var assignments = await _worksOnRepository.GetAllWorksOnAsync();
            var total = assignments.Count();
            return new { AssignmentsTotal = total };
        }

        //List Employee By Department
        public async Task<List<EmployeeListDto>> GetListEmployeeByDepartmentAsync(int pageNumber, int pageSize, string departmentName)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var filteredEmployees = employees
                .Where(e => e.Dept != null && e.Dept.Deptname.Equals(departmentName, StringComparison.OrdinalIgnoreCase))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeListDto
                {
                    Empid = e.Empid,
                    Name = $"{e.Fname} {e.Lname}",
                    Email = e.Email,
                    Position = e.Position,
                    Department = e.Dept.Deptname
                })
                .ToList();

            return filteredEmployees;
        }

        //Report List Employee By Department
        public async Task<byte[]> GetListEmployeeByDepartmentReportAsync(string departmentName)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var filteredEmployees = employees
                .Where(e => e.Dept != null && e.Dept.Deptname.Equals(departmentName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var document = new PdfDocument();

            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Portrait,
                PageSize = PageSize.A4,
                MarginBottom = 8,
                MarginLeft = 8,
                MarginRight = 8,
                MarginTop = 8,
            };

            if (!filteredEmployees.Any())
            {
                var noDataHtml = $@"
                <style>
                    h2 {{ text-align: center; }}
                </style>
                <h2>No employees found in department: {departmentName}</h2>";
                        PdfGenerator.AddPdfPages(document, noDataHtml, config);
            }
            else
            {
                int employeesPerPage = 20;

                for (int i = 0; i < filteredEmployees.Count; i += employeesPerPage)
                {
                    var pageEmployees = filteredEmployees.Skip(i).Take(employeesPerPage);
                    var htmlContent = $@"
                <style>
                    table {{ width: 100%; border-collapse: collapse; margin-bottom: 20px; font-family: Arial, sans-serif; }}
                    th, td {{ padding: 8px 12px; border: 1px solid #ccc; text-align: left; }}
                    th {{ background-color: #007bff; color: white; font-weight: bold; }}
                    tr:nth-child(even) {{ background-color: #f9f9f9; }}
                    tr:nth-child(odd) {{ background-color: #ffffff; }}
                    h2 {{ text-align: center; }}
                </style>
                <h2>List of Employees By Department: {departmentName}</h2>
                <table>
                    <thead>
                        <tr><th>Employee ID</th><th>Name</th><th>Email</th><th>Position</th><th>Department</th></tr>
                    </thead>
                    <tbody>";

                        foreach (var employee in pageEmployees)
                        {
                            htmlContent += $@"
                    <tr>
                        <td>{employee.Empid}</td>
                        <td>{employee.Fname} {employee.Lname}</td>
                        <td>{employee.Email}</td>
                        <td>{employee.Position}</td>
                        <td>{employee.Dept?.Deptname}</td>
                    </tr>";
                        }

                        htmlContent += @"
                    </tbody>
                </table>";

                    PdfGenerator.AddPdfPages(document, htmlContent, config);
                }
            }

            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }

        //Employee Leaves Report
        public async Task<byte[]> GetEmployeeLeavesReportAsync(DateTime startDate, DateTime endDate)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var document = new PdfDocument();

            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Portrait,
                PageSize = PageSize.A4,
                MarginBottom = 8,
                MarginLeft = 8,
                MarginRight = 8,
                MarginTop = 8,
            };

            string htmlContent = @"
            <style>
                table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-bottom: 20px;
                    font-family: Arial, sans-serif;
                }
                th, td {
                    padding: 8px 12px;
                    border: 1px solid #ccc;
                    text-align: left;
                }
                th {
                    background-color: #007bff;
                    color: white;
                    font-weight: bold;
                }
                tr {
                    background-color: #f9f9f9;
                }
                h1{
                    text-align:center
                }
            </style>";

                        htmlContent += @"
            <h1>Employee Leave Requests Summary</h1>
            <h2>From: " + startDate.ToString("d") + @" To: " + endDate.ToString("d") + @"</h2>
            <table>
                <thead>
                    <tr>
                        <th>Leave Type</th>
                        <th>Number of Leave Requests</th>
                    </tr>
                </thead>
                <tbody>";

                    var leaveSummary = employees
                        .SelectMany(e => e.Leaverequests
                            .Where(leave => leave.Startdate >= startDate && leave.Enddate <= endDate)
                            .Select(leave => leave.Leavetype)
                        )
                        .GroupBy(leaveType => leaveType)
                        .Select(g => new
                        {
                            LeaveType = g.Key,
                            RequestCount = g.Count() 
                        })
                        .ToList();

                    if (!leaveSummary.Any())
                    {
                        htmlContent += @"
                    <tr>
                        <td colspan='2' style='text-align:center;'>No items</td>
                    </tr>";
                    }
                    else
                    {
                        foreach (var summary in leaveSummary)
                        {
                            htmlContent += $@"
                    <tr>
                        <td>{summary.LeaveType}</td>
                        <td>{summary.RequestCount}</td>
                    </tr>";
                        }
                    }

                    htmlContent += @"
                </tbody>
            </table>";

            PdfGenerator.AddPdfPages(document, htmlContent, config);

            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }

        //Projects Report
        public async Task<byte[]> GetAllProjectsReportAsync()
        {
            var projects = await _projectRepository.GetAllProjectsAsync();

            var document = new PdfDocument();

            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Portrait,
                PageSize = PageSize.A4,
                MarginBottom = 8,
                MarginLeft = 8,
                MarginRight = 8,
                MarginTop = 8,
            };

            var htmlContent = @"
            <style>
                table { width: 100%; border-collapse: collapse; margin-bottom: 20px; font-family: Arial, sans-serif; }
                th, td { padding: 8px 12px; border: 1px solid #ccc; text-align: left; }
                th { background-color: #007bff; color: white; font-weight: bold; }
                tr:nth-child(even) { background-color: #f9f9f9; }
                tr:nth-child(odd) { background-color: #ffffff; }
                h2 { text-align: center; }
            </style>
            <h2>Projects Report</h2>
            <table>
                <thead>
                    <tr>
                        <th>Project Name</th>
                        <th>Total Hours Logged</th>
                        <th>Number of Employees Involved</th>
                        <th>Average Hours per Employee</th>
                    </tr>
                </thead>
                <tbody>";

            foreach (var project in projects)
            {
                var totalHours = project.Worksons.Sum(w => w.Hoursworked ?? 0);

                var uniqueEmployees = project.Worksons.Select(w => w.Empid).Distinct().Count();

                var averageHours = uniqueEmployees > 0 ? (double)totalHours / uniqueEmployees : 0;

                htmlContent += $@"
                <tr>
                    <td>{project.Projname}</td>
                    <td>{totalHours}</td>
                    <td>{uniqueEmployees}</td>
                    <td>{averageHours:F2}</td>
                </tr>";
                    }

                    htmlContent += @"
                </tbody>
            </table>";

            PdfGenerator.AddPdfPages(document, htmlContent, config);

            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }


    }
}
