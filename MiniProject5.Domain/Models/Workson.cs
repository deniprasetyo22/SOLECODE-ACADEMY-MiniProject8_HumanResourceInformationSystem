using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Application.DTOs;
using MiniProject6.Domain.Models;

namespace MiniProject5.Persistence.Models;

[PrimaryKey("Empid", "Projid")]
[Table("workson")]
public partial class Workson
{
    [Key]
    [Column("empid")]
    public int Empid { get; set; }

    [Key]
    [Column("projid")]
    public int Projid { get; set; }

    [ForeignKey("Projid")]
    [InverseProperty("Worksons")]
    public virtual Project? Proj { get; set; }

    [Column("dateworked")]
    public DateOnly Dateworked { get; set; }

    [Column("hoursworked")]
    public int? Hoursworked { get; set; }

    [ForeignKey("Empid")]
    [InverseProperty("Worksons")]
    [JsonIgnore]
    public virtual Employee? Emp { get; set; }

    [NotMapped]
    public DateOnlyObject? DateworkedObject { get; set; }

    [Column("userid")]
    [StringLength(255)]
    public string? Userid { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("Worksons")]
    [JsonIgnore]
    public virtual AppUser? AppUser { get; set; }

    public void ConvertDobObjectToDateOnly()
    {
        if (DateworkedObject != null)
        {
            Dateworked = new DateOnly(DateworkedObject.Year, DateworkedObject.Month, DateworkedObject.Day);
        }
    }
}
