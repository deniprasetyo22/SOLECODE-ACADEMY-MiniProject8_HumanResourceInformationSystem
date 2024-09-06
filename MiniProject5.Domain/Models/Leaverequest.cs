using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Persistence.Models;

namespace MiniProject6.Persistence.Models;

[Table("leaverequest")]
public partial class Leaverequest
{
    [Key]
    [Column("requestid")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Requestid { get; set; }

    [Column("requestname")]
    [StringLength(255)]
    public string? Requestname { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("startdate", TypeName = "timestamp without time zone")]
    public DateTime? Startdate { get; set; }

    [Column("enddate", TypeName = "timestamp without time zone")]
    public DateTime? Enddate { get; set; }

    [Column("processid")]
    public int? Processid { get; set; }

    [Column("empid")]
    public int? Empid { get; set; }

    [Column("leavetype")]
    [StringLength(255)]
    public string Leavetype { get; set; } = null!;

    [Column("reason")]
    [StringLength(255)]
    public string Reason { get; set; } = null!;

    [ForeignKey("Empid")]
    [InverseProperty("Leaverequests")]
    public virtual Employee? Emp { get; set; }

    [ForeignKey("Processid")]
    [InverseProperty("Leaverequests")]
    public virtual Process? Process { get; set; }
}
