using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MiniProject6.Domain.Models;

namespace MiniProject6.Persistence.Models;

[Table("workflowactions")]
public partial class Workflowaction
{
    [Key]
    [Column("actionid")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Actionid { get; set; }

    [Column("processid")]
    public int? Processid { get; set; }

    [Column("stepid")]
    public int? Stepid { get; set; }

    [Column("actorid")]
    [StringLength(255)]
    public string? Actorid { get; set; }

    [Column("action")]
    [StringLength(255)]
    public string? Action { get; set; }

    [Column("actiondate", TypeName = "timestamp without time zone")]
    public DateTime? Actiondate { get; set; }

    [Column("comments")]
    public string? Comments { get; set; }

    [ForeignKey("Actorid")]
    [InverseProperty("Workflowactions")]
    public virtual AppUser? Actor { get; set; }

    [ForeignKey("Processid")]
    [InverseProperty("Workflowactions")]
    public virtual Process? Process { get; set; }

    [ForeignKey("Stepid")]
    [InverseProperty("Workflowactions")]
    public virtual Workflowsequence? Step { get; set; }
}
