using MiniProject5.Application.DTOs;
using MiniProject5.Application.Interfaces.IRepositories;
using MiniProject5.Application.Interfaces.IServices;
using MiniProject5.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(paginationDto pagination)
        {
            return await _departmentRepository.GetAllDepartmentsAsync(pagination);
        }

        public async Task<Department> GetDepartmentByIdAsync(int deptId)
        {
            return await _departmentRepository.GetDepartmentByIdAsync(deptId);
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            return await _departmentRepository.AddDepartmentAsync(department);
        }

        public async Task UpdateDepartmentAsync(int deptId, Department department)
        {
            await _departmentRepository.UpdateDepartmentAsync(deptId, department);
        }

        public async Task DeleteDepartmentAsync(int deptId)
        {
            await _departmentRepository.DeleteDepartmentAsync(deptId);
        }
    }
}
