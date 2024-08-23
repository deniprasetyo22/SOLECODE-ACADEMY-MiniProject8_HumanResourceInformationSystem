using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MiniProject5.Persistence.Models;

[Table("project")]
public partial class Project
{
    [Key]
    [Column("projid")]
    public int Projid { get; set; }

    [Column("projname")]
    [StringLength(255)]
    public string Projname { get; set; } = null!;

    [Column("deptid")]
    public int Deptid { get; set; }

    [ForeignKey("Deptid")]
    [InverseProperty("Projects")]
    public virtual Department? Dept { get; set; }

    [InverseProperty("Proj")]
    [JsonIgnore]
    public virtual ICollection<Workson>? Worksons { get; set; } = new List<Workson>();
}
