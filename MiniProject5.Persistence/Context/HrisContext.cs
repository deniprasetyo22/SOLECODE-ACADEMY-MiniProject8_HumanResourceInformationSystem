using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Persistence.Models;
using MiniProject6.Domain.Models;
using MiniProject6.Persistence.Models;

namespace MiniProject5.Persistence.Context;

public partial class HrisContext : IdentityDbContext<AppUser>
{

    public HrisContext(DbContextOptions<HrisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Dependent> Dependents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Workson> Worksons { get; set; }

    public virtual DbSet<Nextsteprule> Nextsteprules { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<Workflow> Workflows { get; set; }

    public virtual DbSet<Workflowaction> Workflowactions { get; set; }

    public virtual DbSet<Workflowsequence> Workflowsequences { get; set; }

    public virtual DbSet<Leaverequest> Leaverequests { get; set; }
}
