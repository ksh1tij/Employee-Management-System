using System.Collections.Generic;
using System.Reflection.Emit;
using EmployeePortal.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.API.Data
{
    public class EmployeeManagementContext : DbContext
    {
        public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }
        public DbSet<UserGroupMember> UserGroupMembers { get; set; }
        public DbSet<PerformanceMetric> PerformanceMetrics { get; set; }
        public DbSet<Competency> Competencies { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payslip> Payslips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Payslip>().ToTable("Payslips");

            modelBuilder.Entity<Payslip>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserDepartment>()
                .HasKey(ud => new { ud.UserId, ud.DepartmentId });

            modelBuilder.Entity<UserDepartment>()
                .HasOne(ud => ud.Department)
                .WithMany(d => d.UserDepartments)
                .HasForeignKey(ud => ud.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGroupMember>()
                .HasKey(ugm => new { ugm.UserId, ugm.GroupId });

            modelBuilder.Entity<UserGroupMember>()
                .HasOne(ugm => ugm.User)
                .WithMany(u => u.UserGroupMembers)
                .HasForeignKey(ugm => ugm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.PerformanceMetrics)
                .WithOne(pm => pm.User)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Competencies)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.UserDepartments)
                .WithOne(ud => ud.Department)
                .HasForeignKey(ud => ud.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Competency>()
                .HasOne(c => c.User)
                .WithMany(u => u.Competencies)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Competency>()
                .HasOne(c => c.Manager)
                .WithMany()
                .HasForeignKey(c => c.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PerformanceMetric>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.PerformanceMetrics)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PerformanceMetric>()
                .HasOne(pm => pm.Manager)
                .WithMany()
                .HasForeignKey(pm => pm.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
