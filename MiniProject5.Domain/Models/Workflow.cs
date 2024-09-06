using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniProject6.Persistence.Models;

[Table("workflow")]
public partial class Workflow
{
    [Key]
    [Column("workflowid")]
    public int Workflowid { get; set; }

    [Column("workflowname")]
    [StringLength(255)]
    public string? Workflowname { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [InverseProperty("Workflow")]
    public virtual ICollection<Process> Processes { get; set; } = new List<Process>();

    [InverseProperty("Workflow")]
    public virtual ICollection<Workflowsequence> Workflowsequences { get; set; } = new List<Workflowsequence>();
}
