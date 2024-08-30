using MiniProject5.Application.DTOs;
using MiniProject5.Persistence.Models;
using MiniProject6.Application.DTOs;
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
        Task UpdateOwnProfile(int empId, EmployeeDto employeeDto);
    }
}
