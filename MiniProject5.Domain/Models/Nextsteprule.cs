using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniProject6.Persistence.Models;

[Table("nextsteprules")]
public partial class Nextsteprule
{
    [Key]
    [Column("ruleid")]
    public int Ruleid { get; set; }

    [Column("currentstepid")]
    public int? Currentstepid { get; set; }

    [Column("nextstepid")]
    public int? Nextstepid { get; set; }

    [Column("conditiontype")]
    [StringLength(255)]
    public string? Conditiontype { get; set; }

    [Column("conditionvalue")]
    [StringLength(255)]
    public string? Conditionvalue { get; set; }

    [ForeignKey("Currentstepid")]
    [InverseProperty("NextstepruleCurrentsteps")]
    public virtual Workflowsequence? Currentstep { get; set; }

    [ForeignKey("Nextstepid")]
    [InverseProperty("NextstepruleNextsteps")]
    public virtual Workflowsequence? Nextstep { get; set; }
}
