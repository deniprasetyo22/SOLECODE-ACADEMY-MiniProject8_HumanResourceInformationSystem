using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Application.DTOs;
using MiniProject6.Domain.Models;

namespace MiniProject5.Persistence.Models;

[Table("employee")]
[Index("Ssn", Name = "employee_ssn_key", IsUnique = true)]
public partial class Employee
{
    [Key]
    [Column("empid")]
    public int Empid { get; set; }

    [Column("fname")]
    [StringLength(255)]
    public string Fname { get; set; } = null!;

    [Column("lname")]
    [StringLength(255)]
    public string? Lname { get; set; }

    [Column("ssn")]
    [StringLength(255)]
    public string Ssn { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("address")]
    [StringLength(255)]
    public string Address { get; set; } = null!;

    [Column("position")]
    [StringLength(255)]
    public string Position { get; set; } = null!;

    [Column("salary")]
    public double Salary { get; set; }

    [Column("sex")]
    [StringLength(255)]
    public string Sex { get; set; } = null!;

    [Column("dob")]
    public DateOnly Dob { get; set; }

    [Column("phoneno")]
    [StringLength(255)]
    public string Phoneno { get; set; } = null!;

    [Column("emptype")]
    [StringLength(255)]
    public string Emptype { get; set; } = null!;

    [Column("level")]
    public int? Level { get; set; }

    [Column("deptid")]
    public int? Deptid { get; set; }

    [Column("supervisorid")]
    public int? SupervisorId { get; set; }

    [ForeignKey("SupervisorId")]
    [InverseProperty("Subordinates")]
    [JsonIgnore]
    public virtual Employee? Supervisor { get; set; }

    [InverseProperty("Supervisor")]
    public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();

    [Column("status")]
    [StringLength(255)]
    public string? Status { get; set; }

    [Column("reason")]
    [StringLength(255)]
    public string? Reason { get; set; }

    [Column("lastupdateddate", TypeName = "timestamp without time zone")]
    public DateTime? Lastupdateddate { get; set; } = DateTime.Now;

    [InverseProperty("Mgremp")]
    [JsonIgnore]
    public virtual ICollection<Department>? Departments { get; set; }

    [InverseProperty("Emp")]
    public virtual ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();

    [ForeignKey("Deptid")]
    [InverseProperty("Employees")]
    [JsonIgnore]
    public virtual Department? Dept { get; set; }

    [InverseProperty("Emp")]
    public virtual ICollection<Workson> Worksons { get; set; } = new List<Workson>();

    [NotMapped]
    public DateOnlyObject? DobObject { get; set; }

    public void ConvertDobObjectToDateOnly()
    {
        if (DobObject != null)
        {
            Dob = new DateOnly(DobObject.Year, DobObject.Month, DobObject.Day);
        }
    }

    [Column("userId")]
    [StringLength(255)]
    public string? userId { get; set; }

    [ForeignKey("userId")]
    [InverseProperty("Employee")]
    public virtual AppUser? AppUser { get; set; }

}
