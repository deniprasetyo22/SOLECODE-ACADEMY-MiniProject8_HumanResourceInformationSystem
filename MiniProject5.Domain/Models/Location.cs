using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MiniProject5.Persistence.Models;

[Table("location")]
public partial class Location
{
    [Key]
    [Column("locationid")]
    public int Locationid { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string Address { get; set; } = null!;

    [Column("deptid")]
    public int? Deptid { get; set; }

    [ForeignKey("Deptid")]
    [InverseProperty("Locations")]
    [JsonIgnore]
    public virtual Department? Dept { get; set; }
}
