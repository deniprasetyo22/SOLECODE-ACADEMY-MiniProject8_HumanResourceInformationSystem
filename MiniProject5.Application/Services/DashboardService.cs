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

namespace MiniProject8.Application.Services
{
    public class DashboardService:IDashboardService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public DashboardService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        public async Task<IList<EmployeeDistributionByDeptDto>> GetEmployeePercentageByDepartmentAsync()
        {
            return await _employeeRepository.GetEmployeePercentageByDepartmentAsync();
        }

        public async Task<IList<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync()
        {
            return await _departmentRepository.GetAverageSalaryByDepartmentAsync();
        }


        public async Task<byte[]> GenerateEmployeeReportPdfAsync(int? departmentId, int pageNumber, int pageSize)
        {
            var employees = await _employeeRepository.GetEmployeesByDepartmentAsync(departmentId, pageNumber, pageSize);
            var totalCount = await _employeeRepository.GetTotalCountByDepartmentAsync(departmentId);

            string htmlContent = "<table>";
            htmlContent += "<tr><td>Empid</td><td>First Name</td><td>Last Name</td><td>Email</td><td>Position</td><td>Salary</td></tr>";

            foreach (var employee in employees)
            {
                htmlContent += "<tr style='border:1px solid #ccc;'>";
                htmlContent += $"<td>{employee.Empid}</td>";
                htmlContent += $"<td>{employee.Fname}</td>";
                htmlContent += $"<td>{employee.Lname}</td>";
                htmlContent += $"<td>{employee.Email}</td>";
                htmlContent += $"<td>{employee.Position}</td>";
                htmlContent += $"<td>{employee.Salary.ToString("F2")}</td>";
                htmlContent += "</tr>";
            }

            htmlContent += "</table>";

            var document = new PdfDocument();

            var config = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Landscape,
                PageSize = PageSize.A4
            };

            string cssStr = File.ReadAllText(@"./Style/ReportStyle.css");
            CssData css = PdfGenerator.ParseStyleSheet(cssStr);

            PdfGenerator.AddPdfPages(document, htmlContent, config, css);

            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }
    }
}
