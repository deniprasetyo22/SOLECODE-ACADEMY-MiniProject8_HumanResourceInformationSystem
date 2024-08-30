using Microsoft.EntityFrameworkCore;
using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Persistence.Models;
using MiniProject6.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(paginationDto pagination)
        {
            return await _employeeRepository.GetAllEmployeesAsync(pagination);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int empId)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(empId);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            return await _employeeRepository.AddEmployeeAsync(employee);
            
        }

        public async Task UpdateEmployeeAsync(int empId, Employee employee)
        {
            await _employeeRepository.UpdateEmployeeAsync(empId, employee);
        }

        public async Task DeactivateEmployeeAsync(int empId, string reason)
        {
            await _employeeRepository.DeactivateEmployeeAsync(empId, reason);
        }

        public async Task DeleteEmployeeAsync(int empId)
        {
            await _employeeRepository.DeleteEmployeeAsync(empId);
        }

        public async Task<IEnumerable<Employee>> SearchEmployee(searchDto search, paginationDto pagination)
        {
            return await _employeeRepository.SearchEmployee(search, pagination);
        }

        public async Task<IEnumerable<Employee>> GetSupervisedEmployeesAsync(int supervisorId)
        {
            return await _employeeRepository.GetSupervisedEmployeesAsync(supervisorId);
        }

        public async Task<EmployeeDto> GetOwnProfile()
        {
            return await _employeeRepository.GetOwnProfile();
        }

        public async Task UpdateOwnProfile(int empId, EmployeeDto employeeDto)
        {
            await _employeeRepository.UpdateOwnProfile(empId, employeeDto);
        }
    }
}
