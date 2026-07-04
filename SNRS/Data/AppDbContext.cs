using Microsoft.EntityFrameworkCore;
using SNRS.Models;

namespace SNRS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName).IsUnique();

            // Category -> Resources (1-to-many)
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            // Resource -> Projects (1-to-many)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Resource)
                .WithMany(r => r.Projects)
                .HasForeignKey(p => p.ResourceID)
                .OnDelete(DeleteBehavior.Cascade);

            // Project -> Reports (1-to-many)
            modelBuilder.Entity<Report>()
                .HasOne(rp => rp.Project)
                .WithMany(p => p.Reports)
                .HasForeignKey(rp => rp.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Reports (1-to-many, Restrict avoids multiple cascade paths error)
            modelBuilder.Entity<Report>()
                .HasOne(rp => rp.GeneratedBy)
                .WithMany(u => u.Reports)
                .HasForeignKey(rp => rp.GeneratedByID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}