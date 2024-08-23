using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MiniProject5.Persistence.Models;

namespace MiniProject5.Persistence.Context;

public partial class HrisContext : DbContext
{
    public HrisContext()
    {
    }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Deptid).HasName("department_pkey");

            entity.HasOne(d => d.Mgremp).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_mgrempid");
        });

        modelBuilder.Entity<Dependent>(entity =>
        {
            entity.HasKey(e => e.Dependentid).HasName("dependent_pkey");

            entity.HasOne(d => d.Emp).WithMany(p => p.Dependents)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_employee");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Empid).HasName("employee_pkey");

            entity.Property(e => e.Lastupdateddate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Dept).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_department");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Locationid).HasName("location_pkey");

            entity.HasOne(d => d.Dept).WithMany(p => p.Locations)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_department");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Projid).HasName("project_pkey");

            entity.HasOne(d => d.Dept).WithMany(p => p.Projects).HasConstraintName("fk_deptid");
        });

        modelBuilder.Entity<Workson>(entity =>
        {
            entity.HasKey(e => new { e.Empid, e.Projid }).HasName("workson_pkey");

            entity.HasOne(d => d.Emp).WithMany(p => p.Worksons).HasConstraintName("workson_empid_fkey");

            entity.HasOne(d => d.Proj).WithMany(p => p.Worksons).HasConstraintName("workson_projid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
