using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using task_manager.Models;

namespace task_manager.Data;

public partial class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tasks> Tasks { get; set; }

    public  DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tasks>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd()
            .HasAnnotation("SqlServer:Identity", "1, 1");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.TaskStatus).IsRequired();
            entity.Property(e => e.TaskPriority).IsRequired();
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:Identity", "1, 1");
            entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
            entity.Property(e => e.Password).IsRequired();
            entity.HasMany(e => e.Tasks).WithOne(e => e.User).HasForeignKey(e => e.userId);
        });

        base.OnModelCreating(modelBuilder);

    }

}
