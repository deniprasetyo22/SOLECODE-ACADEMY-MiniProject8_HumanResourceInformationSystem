using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class TopEmployeeDto
    {
        public int EmpId { get; set; }
        public string? EmployeeName { get; set; }
        public int TotalHoursWorked { get; set; }
    }
}
