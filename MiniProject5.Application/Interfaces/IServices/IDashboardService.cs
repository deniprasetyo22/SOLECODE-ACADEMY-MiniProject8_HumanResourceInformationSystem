using MiniProject8.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Interfaces.IServices
{
    public interface IDashboardService
    {
        //Employee Distribution By Department
        Task<List<EmployeeDistributionByDeptDto>> GetEmployeeDistributionByDepartmentAsync();

        //Top 5 Employees By Performance
        Task<List<TopEmployeeDto>> GetTopEmployeesByPerformanceAsync();

        //Average salary by department
        Task<List<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync();

        //Workflow Process
        Task<object> GetWorkflowProcessesAsync();

        //Employees Total
        Task<object> GetEmployeesTotalAsync();

        //Departments Total
        Task<object> GetDepartmentsTotalAsync();

        //Projects Total
        Task<object> GetProjectsTotalAsync();

        //Assignments Total
        Task<object> GetAssignmentsTotalAsync();

        //List Employee By Department
        Task<List<EmployeeListDto>> GetListEmployeeByDepartmentAsync(int pageNumber, int pageSize, string departmentName);

        //List Employee By Department Report
        Task<byte[]> GetListEmployeeByDepartmentReportAsync(string departmentName);

        //Employee Leaves Report
        Task<byte[]> GetEmployeeLeavesReportAsync(DateTime startDate, DateTime endDate);

        //Project Report
        Task<byte[]> GetAllProjectsReportAsync();

    }
}
