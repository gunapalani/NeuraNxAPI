using Microsoft.EntityFrameworkCore;
using NeuraNx.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Board configuration
            modelBuilder.Entity<Board>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name).IsRequired().HasMaxLength(200);
                entity.HasIndex(b => b.Name).IsUnique();
            });

            // TaskItem configuration
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(500);
                entity.Property(t => t.Description).HasMaxLength(2000);
                entity.Property(t => t.Status).IsRequired();
                entity.Property(t => t.Priority).IsRequired();
                entity.Property(t => t.RowVersion).IsConcurrencyToken();

                // Relationships
                entity.HasOne(t => t.Board)
                      .WithMany(b => b.TaskItems)
                      .HasForeignKey(t => t.BoardId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.AssignedTo)
                      .WithMany()
                      .HasForeignKey(t => t.AssignedToId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Indexes
                entity.HasIndex(t => t.BoardId);
                entity.HasIndex(t => t.AssignedToId);
                entity.HasIndex(t => t.Status);
                entity.HasIndex(t => t.Priority);
            });

            // Comment configuration
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Content).IsRequired().HasMaxLength(1000);

                // Relationships
                entity.HasOne(c => c.TaskItem)
                      .WithMany(t => t.Comments)
                      .HasForeignKey(c => c.TaskItemId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.CreatedBy)
                      .WithMany()
                      .HasForeignKey(c => c.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(c => c.TaskItemId);
                entity.HasIndex(c => c.CreatedById);
            });

            // Employee configuration (ensure no conflicts)
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Designation).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
