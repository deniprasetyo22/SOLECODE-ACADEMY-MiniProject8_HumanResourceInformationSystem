using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MiniProject6.Domain.Models;

namespace MiniProject6.Persistence.Models;

[Table("workflowsequences")]
public partial class Workflowsequence
{
    [Key]
    [Column("stepid")]
    public int Stepid { get; set; }

    [Column("workflowid")]
    public int? Workflowid { get; set; }

    [Column("steporder")]
    public int? Steporder { get; set; }

    [Column("stepname")]
    [StringLength(255)]
    public string? Stepname { get; set; }

    [Column("requiredrole")]
    [StringLength(255)]
    public string? Requiredrole { get; set; }

    [InverseProperty("Currentstep")]
    public virtual ICollection<Nextsteprule> NextstepruleCurrentsteps { get; set; } = new List<Nextsteprule>();

    [InverseProperty("Nextstep")]
    public virtual ICollection<Nextsteprule> NextstepruleNextsteps { get; set; } = new List<Nextsteprule>();

    [InverseProperty("Currentstep")]
    public virtual ICollection<Process> Processes { get; set; } = new List<Process>();

    [ForeignKey("Requiredrole")]
    [InverseProperty("Workflowsequences")]
    public virtual AppUser? RequiredroleNavigation { get; set; }

    [ForeignKey("Workflowid")]
    [InverseProperty("Workflowsequences")]
    public virtual Workflow? Workflow { get; set; }

    [InverseProperty("Step")]
    public virtual ICollection<Workflowaction> Workflowactions { get; set; } = new List<Workflowaction>();
}
