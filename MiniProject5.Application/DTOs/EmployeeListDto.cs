using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject8.Application.DTOs
{
    public class EmployeeListDto
    {
        public int Empid { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
    }

}
