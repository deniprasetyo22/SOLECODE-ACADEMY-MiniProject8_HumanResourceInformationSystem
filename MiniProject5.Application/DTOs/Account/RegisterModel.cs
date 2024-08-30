
using MiniProject5.Persistence.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject6.Application.DTOs.Account
{
    public class RegisterModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

        // Employee-specific properties
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Ssn { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Position { get; set; } = null!;
        public double Salary { get; set; }
        public string Sex { get; set; } = null!;
        public DateOnly Dob { get; set; }
        public string Phoneno { get; set; } = null!;
        public string Emptype { get; set; } = null!;
        public int? Level { get; set; }
        public int? Deptid { get; set; }
        public int? SupervisorId { get; set; }
        [RegularExpression("Administrator|HR Manager|Department Manager|Employee Supervisor|Employee",
          ErrorMessage = "Role must be one of the following: Administrator, HR Manager, Department Manager, Employee Supervisor, Employee.")]
        public string? Role { get; set; }
        // Dependents
        public List<Dependent> Dependents { get; set; } = new List<Dependent>();

    }
}
