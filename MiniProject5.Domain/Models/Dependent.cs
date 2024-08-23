using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Application.DTOs;

namespace MiniProject5.Persistence.Models;

[Table("dependent")]
public partial class Dependent
{
    [Key]
    [Column("dependentid")]
    public int Dependentid { get; set; }

    [Column("fname")]
    [StringLength(255)]
    public string fName { get; set; } = null!;
    
    [Column("lname")]
    [StringLength(255)]
    public string lName { get; set; }

    [Column("sex")]
    [StringLength(255)]
    public string Sex { get; set; } = null!;

    [Column("dob")]
    public DateOnly Dob { get; set; }

    [Column("relationship")]
    [StringLength(255)]
    public string Relationship { get; set; } = null!;

    [Column("empid")]
    public int? Empid { get; set; }

    [ForeignKey("Empid")]
    [InverseProperty("Dependents")]
    [JsonIgnore]
    public virtual Employee? Emp { get; set; }

    [NotMapped]
    public DateOnlyObject? DobObject { get; set; }

    public void ConvertDobObjectToDateOnly()
    {
        if (DobObject != null)
        {
            Dob = new DateOnly(DobObject.Year, DobObject.Month, DobObject.Day);
        }
    }
}
