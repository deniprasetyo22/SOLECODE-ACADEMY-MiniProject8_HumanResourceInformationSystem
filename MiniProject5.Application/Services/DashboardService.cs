using MiniProject5.Application.Interfaces.IRepositories;
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
using MiniProject5.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using MiniProject6.Application.Interfaces.IRepositories;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace MiniProject8.Application.Services
{
    public class DashboardService:IDashboardService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWorksOnRepository _worksOnRepository;
        private readonly IWorkflowRepository _workflowRepository;
        public DashboardService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IWorksOnRepository worksOnRepository, IWorkflowRepository workflowRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _worksOnRepository = worksOnRepository;
            _workflowRepository = workflowRepository;
        }

        //Employee Distribution By Department
        public async Task<List<EmployeeDistributionByDeptDto>> GetEmployeePercentageByDepartmentAsync()
        {
            return await _employeeRepository.GetEmployeePercentageByDepartmentAsync();
        }

        //Top 5 employees by performance
        public async Task<List<TopEmployeeDto>> GetTopEmployeesByPerformanceAsync()
        {
            return await _worksOnRepository.GetTopEmployeesByPerformanceAsync();
        }

        //Average salary by department
        public async Task<List<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync()
        {
            return await _employeeRepository.GetAverageSalaryByDepartmentAsync();
        }

        //Workflow Process
        public async Task<List<ProcessDto>> GetAllProcessesAsync()
        {
            return await _workflowRepository.GetAllProcessesAsync();
        }

        //List Employee Leaves
        public async Task<List<EmployeeLeaveDto>> GetEmployeeLeavesAsync(DateTime startDate, DateTime endDate)
        {
            return await _employeeRepository.GetEmployeeLeavesAsync(startDate, endDate);
        }

        //List Employee By Department
        public async Task<List<EmployeeListDto>> GetListEmployeeByDepartment(int pageNumber, int pageSize, string departmentName)
        {
            return await _employeeRepository.GetListEmployeeByDepartment(pageNumber, pageSize, departmentName);
        }

        //Report List Employee By Department
        public async Task<byte[]> GetReportListEmployeeByDepartment(string departmentName)
        {
            var employees = await _employeeRepository.GetReportListEmployeeByDepartment(departmentName);

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

            int employeesPerPage = 20;

            for (int i = 0; i < employees.Count; i += employeesPerPage)
            {
                var pageEmployees = employees.Skip(i).Take(employeesPerPage);
                string htmlContent = @"
                <style>
                    table { width: 100%; border-collapse: collapse; margin-bottom: 20px; font-family: Arial, sans-serif; }
                    th, td { padding: 8px 12px; border: 1px solid #ccc; text-align: left; }
                    th { background-color: #007bff; color: white; font-weight: bold; }
                    tr:nth-child(even) { background-color: #f9f9f9; }
                    tr:nth-child(odd) { background-color: #ffffff; }
                </style>
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
                        <td>{employee.Name}</td>
                        <td>{employee.Email}</td>
                        <td>{employee.Position}</td>
                        <td>{employee.Department}</td>
                    </tr>";
                }

                htmlContent += @"
                    </tbody>
                </table>";

                PdfGenerator.AddPdfPages(document, htmlContent, config);
            }
            
            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }
        
        //Report Employee Leaves 
        public async Task<byte[]> GetReportEmployeeLeavesAsync(DateTime startDate, DateTime endDate)
        {
            var employeeLeaves = await _employeeRepository.GetReportEmployeeLeavesAsync(startDate, endDate);

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
            </style>";

            htmlContent += @"
            <table>
                <thead>
                    <tr>
                        <th>Leave Type</th>
                        <th>Total Leaves</th>
                    </tr>
                </thead>
                <tbody>";

            foreach (var leaveGroup in employeeLeaves)
            {
                htmlContent += $@"
                <tr>
                    <td>{leaveGroup.LeavesType}</td>
                    <td>{leaveGroup.LeavesTotal}</td>
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
