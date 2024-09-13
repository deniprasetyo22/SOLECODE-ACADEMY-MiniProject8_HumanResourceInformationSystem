using MiniProject5.Application.DTOs;
using MiniProject5.Persistence.Models;
using MiniProject6.Application.DTOs;
using MiniProject8.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Interfaces.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(paginationDto pagination);
        Task<Employee> GetEmployeeByIdAsync(int empId);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(int empId, Employee employee);
        Task DeactivateEmployeeAsync(int empId, string reason);
        Task DeleteEmployeeAsync(int empId);
        Task<IEnumerable<Employee>> SearchEmployee(searchDto search, paginationDto pagination);
        Task<IEnumerable<Employee>> GetSupervisedEmployeesAsync(int supervisorId);
        Task<EmployeeDto> GetOwnProfile();
        Task<bool> UpdateOwnProfile(EmployeeDto employeeDto);

        //Employee Distribution By Department
        Task<List<EmployeeDistributionByDeptDto>> GetEmployeePercentageByDepartmentAsync();

        //Average salary by department
        Task<List<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync();

        //List Employee By Department
        Task<List<EmployeeListDto>> GetListEmployeeByDepartment(int pageNumber, int pageSize, string departmentName);

        //Employee Leaves
        Task<List<EmployeeLeaveDto>> GetEmployeeLeavesAsync(DateTime startDate, DateTime endDate);

        //Report List Employee By Department
        Task<List<EmployeeListDto>> GetReportListEmployeeByDepartment(string departmentName);

        //Report Employee Leaves
        Task<List<EmployeeLeaveDto>> GetReportEmployeeLeavesAsync(DateTime startDate, DateTime endDate);

    }
}
