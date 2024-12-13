using MiniProject8.Application.DTOs;
using MiniProject8.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.Interfaces.IServices
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsNoPagesAsync();
        Task<Department> GetDepartmentByIdAsync(int deptId);
        Task<Department> AddDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(int deptId, Department department);
        Task DeleteDepartmentAsync(int deptId);
        Task<object> GetAllDepartmentsAsync(QueryObjectDepartment query);
    }
}
