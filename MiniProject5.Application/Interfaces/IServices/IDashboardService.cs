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
        Task<IList<EmployeeDistributionByDeptDto>> GetEmployeePercentageByDepartmentAsync();
        Task<IList<AverageSalaryDto>> GetAverageSalaryByDepartmentAsync();
        Task<byte[]> GenerateEmployeeReportPdfAsync(int? departmentId, int pageNumber, int pageSize);
    }
}
