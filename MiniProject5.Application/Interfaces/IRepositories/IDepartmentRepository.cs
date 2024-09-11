using MiniProject5.Application.DTOs;
using MiniProject5.Persistence.Models;
using MiniProject8.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject5.Application.Interfaces.IRepositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(paginationDto pagination);
        Task<Department> GetDepartmentByIdAsync(int deptId);
        Task<Department> AddDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(int deptId, Department department);
        Task DeleteDepartmentAsync(int deptId);
        Task<IList<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync();
    }
}
