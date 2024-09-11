using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class EmployeeDistributionByDeptDto
    {
        public string DeptName { get; set; } = null!;
        public int EmployeeCount { get; set; }
        public double Percentage { get; set; }
    }
}
