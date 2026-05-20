// Building Company App - Database Context
// This file needs to be completed based on your project setup

using BuildingCompanyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingCompanyApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectWorker> ProjectWorkers { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Worker configuration
            modelBuilder.Entity<Worker>()
                .HasKey(w => w.WorkerId);

            modelBuilder.Entity<Worker>()
                .HasOne(w => w.User)
                .WithOne(u => u.Worker)
                .HasForeignKey<Worker>(w => w.UserId);

            // Project configuration
            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectId);

            // ProjectWorker configuration
            modelBuilder.Entity<ProjectWorker>()
                .HasKey(pw => pw.ProjectWorkerId);

            modelBuilder.Entity<ProjectWorker>()
                .HasOne(pw => pw.Project)
                .WithMany(p => p.ProjectWorkers)
                .HasForeignKey(pw => pw.ProjectId);

            modelBuilder.Entity<ProjectWorker>()
                .HasOne(pw => pw.Worker)
                .WithMany(w => w.ProjectWorkers)
                .HasForeignKey(pw => pw.WorkerId);

            // ProjectImage configuration
            modelBuilder.Entity<ProjectImage>()
                .HasKey(pi => pi.ImageId);

            modelBuilder.Entity<ProjectImage>()
                .HasOne(pi => pi.Project)
                .WithMany(p => p.ProjectImages)
                .HasForeignKey(pi => pi.ProjectId);

            // Document configuration
            modelBuilder.Entity<Document>()
                .HasKey(d => d.DocumentId);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.ProjectId)
                .IsRequired(false);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Worker)
                .WithMany()
                .HasForeignKey(d => d.WorkerId)
                .IsRequired(false);
        }
    }
}
