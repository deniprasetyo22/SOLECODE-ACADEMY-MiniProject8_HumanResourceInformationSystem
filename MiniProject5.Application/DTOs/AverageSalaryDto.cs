using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class AverageSalaryDto
    {
        public string DeptName { get; set; } = null!;
        public double AverageSalary { get; set; }
    }
}
