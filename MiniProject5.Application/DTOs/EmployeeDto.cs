using MiniProject5.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Application.DTOs
{
    public class EmployeeDto
    {
        public int Empid { get; set; }
        public string Fname { get; set; } = null!;
        public string? Lname { get; set; }
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public DateOnly Dob { get; set; }
        public string Phoneno { get; set; } = null!;
        public string Emptype { get; set; } = null!;
        public int? Level { get; set; }
        public int? Deptid { get; set; }
        public int? SupervisorId { get; set; }
        public string? Status { get; set; }
        public string? Reason { get; set; }
        public DateTime? Lastupdateddate { get; set; } = DateTime.Now;
        public ICollection<DependentDto>? Dependents { get; set; } = new List<DependentDto>();
    }

    public class DependentDto
    {
        public int Dependentid { get; set; }
        public string fName { get; set; } = null!;
        public string lName { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public DateOnly Dob { get; set; }
        public string Relationship { get; set; } = null!;
    }


}
