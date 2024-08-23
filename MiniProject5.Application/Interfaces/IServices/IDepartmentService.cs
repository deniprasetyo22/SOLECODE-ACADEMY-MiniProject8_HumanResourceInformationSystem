using MiniProject5.Application.DTOs;
using MiniProject5.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Interfaces.IServices
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(paginationDto pagination);
        Task<Department> GetDepartmentByIdAsync(int deptId);
        Task<Department> AddDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(int deptId, Department department);
        Task DeleteDepartmentAsync(int deptId);
    }
}
