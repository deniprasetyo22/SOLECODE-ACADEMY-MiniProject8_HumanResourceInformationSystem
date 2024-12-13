using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Interfaces.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int empId);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(int empId, Employee employee);
        Task DeactivateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int empId);
        Task<Employee> GetEmployeeByUserIdAsync(string userId);
        Task DeleteDependentAsync(int dependentId);
        Task<IEnumerable<Employee>> GetSupervisedEmployeesAsync(int supervisorId);

        ////Employee Distribution By Department
        //Task<List<EmployeeDistributionByDeptDto>> GetEmployeePercentageByDepartmentAsync();

        ////Average salary by department
        //Task<List<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync();

        ////List Employee By Department
        //Task<List<EmployeeListDto>> GetListEmployeeByDepartment(int pageNumber, int pageSize, string departmentName);

        ////Employee Leaves
        //Task<List<EmployeeLeaveDto>> GetEmployeeLeavesAsync(DateTime startDate, DateTime endDate);

        ////Report List Employee By Department
        //Task<List<EmployeeListDto>> GetReportListEmployeeByDepartment(string departmentName);

        ////Report Employee Leaves
        //Task<List<EmployeeLeaveDto>> GetReportEmployeeLeavesAsync(DateTime startDate, DateTime endDate);

    }
}
