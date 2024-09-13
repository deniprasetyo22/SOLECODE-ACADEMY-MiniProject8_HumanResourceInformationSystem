using MiniProject5.Application.Interfaces.IRepositories;
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
        Task<List<EmployeeDistributionByDeptDto>> GetEmployeePercentageByDepartmentAsync();

        //Average salary by department
        Task<List<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync();

        //Top Employees By Performance
        Task<List<TopEmployeeDto>> GetTopEmployeesByPerformanceAsync();

        //Workflow Process
        Task<List<ProcessDto>> GetAllProcessesAsync();

        //Employee Leaves
        Task<List<EmployeeLeaveDto>> GetEmployeeLeavesAsync(DateTime startDate, DateTime endDate);

        //List Employee By Department
        Task<List<EmployeeListDto>> GetListEmployeeByDepartment(int pageNumber, int pageSize, string departmentName);
        
        //Report List Employee By Department
        Task<byte[]> GetReportListEmployeeByDepartment(string departmentName);

        //Report Employee Leaves
        Task<byte[]> GetReportEmployeeLeavesAsync(DateTime startDate, DateTime endDate);

    }
}
