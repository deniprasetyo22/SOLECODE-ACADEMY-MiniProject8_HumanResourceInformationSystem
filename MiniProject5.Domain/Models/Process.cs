using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MiniProject6.Domain.Models;

namespace MiniProject6.Persistence.Models;

[Table("process")]
public partial class Process
{
    [Key]
    [Column("processid")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Processid { get; set; }

    [Column("workflowid")]
    public int? Workflowid { get; set; }

    [Column("requesterid")]
    [StringLength(255)]
    public string? Requesterid { get; set; }

    [Column("requesttype")]
    [StringLength(255)]
    public string? Requesttype { get; set; }

    [Column("status")]
    [StringLength(255)]
    public string? Status { get; set; }

    [Column("currentstepid")]
    public int? Currentstepid { get; set; }

    [Column("requestdate", TypeName = "timestamp without time zone")]
    public DateTime? Requestdate { get; set; }

    [ForeignKey("Currentstepid")]
    [InverseProperty("Processes")]
    public virtual Workflowsequence? Currentstep { get; set; }

    [InverseProperty("Process")]
    public virtual ICollection<Leaverequest> Leaverequests { get; set; } = new List<Leaverequest>();

    [ForeignKey("Requesterid")]
    [InverseProperty("Processes")]
    public virtual AppUser? Requester { get; set; }

    [ForeignKey("Workflowid")]
    [InverseProperty("Processes")]
    public virtual Workflow? Workflow { get; set; }

    [InverseProperty("Process")]
    public virtual ICollection<Workflowaction> Workflowactions { get; set; } = new List<Workflowaction>();
}
