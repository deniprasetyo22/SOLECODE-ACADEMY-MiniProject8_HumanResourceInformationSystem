using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniProject5.Persistence.Models;

[Table("department")]
public partial class Department
{
    [Key]
    [Column("deptid")]
    public int Deptid { get; set; }

    [Column("deptname")]
    [StringLength(255)]
    public string Deptname { get; set; } = null!;

    [Column("mgrempid")]
    public int? Mgrempid { get; set; }

    [InverseProperty("Dept")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("Dept")]
    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    [ForeignKey("Mgrempid")]
    [InverseProperty("Departments")]
    public virtual Employee? Mgremp { get; set; }

    [InverseProperty("Dept")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
