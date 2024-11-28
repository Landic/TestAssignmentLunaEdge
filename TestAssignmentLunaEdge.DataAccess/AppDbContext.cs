using Microsoft.EntityFrameworkCore;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TaskModel> Tasks { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.ID);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.CreatedAt).ValueGeneratedOnAdd();
                entity.Property(u => u.UpdatedAt).ValueGeneratedOnAddOrUpdate();
            });
            modelBuilder.Entity<TaskModel>(entity =>
            {
                entity.HasKey(t => t.ID);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(100);
                entity.Property(t => t.Description).HasMaxLength(500);
                entity.Property(t => t.Status).IsRequired();
                entity.Property(t => t.Priority).IsRequired();
                entity.Property(t => t.CreatedAt).ValueGeneratedOnAdd();
                entity.Property(t => t.UpdatedAt).ValueGeneratedOnAddOrUpdate();

                entity.HasOne(t => t.User).WithMany() .HasForeignKey(t => t.UserID).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
