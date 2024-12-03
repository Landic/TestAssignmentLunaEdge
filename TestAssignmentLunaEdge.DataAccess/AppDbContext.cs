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
            // Configuring the User entity
            modelBuilder.Entity<User>(entity =>
            {
                // Setting the primary key of the User table
                entity.HasKey(u => u.ID);

                // Creating unique index on Username and Email fields
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                // Setting properties of the User entity
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50); // Username is required and has max length of 50
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);    // Email is required and has max length of 100
                entity.Property(u => u.PasswordHash).IsRequired();               // Password hash is required
                entity.Property(u => u.CreatedAt).ValueGeneratedOnAdd();        // CreatedAt is auto-generated on insert
                entity.Property(u => u.UpdatedAt).ValueGeneratedOnAddOrUpdate(); // UpdatedAt is auto-generated on insert or update
            });

            // Configuring the TaskModel entity
            modelBuilder.Entity<TaskModel>(entity =>
            {
                // Setting the primary key of the TaskModel table
                entity.HasKey(t => t.ID);

                // Setting properties of the TaskModel entity
                entity.Property(t => t.Title).IsRequired().HasMaxLength(100);   // Title is required and has max length of 100
                entity.Property(t => t.Description).HasMaxLength(500);          // Description has max length of 500
                entity.Property(t => t.Status).IsRequired();                    // Status is required
                entity.Property(t => t.Priority).IsRequired();                  // Priority is required
                entity.Property(t => t.CreatedAt).ValueGeneratedOnAdd();       // CreatedAt is auto-generated on insert
                entity.Property(t => t.UpdatedAt).ValueGeneratedOnAddOrUpdate(); // UpdatedAt is auto-generated on insert or update

                // Configuring the relationship between TaskModel and User entities
                // Each TaskModel is related to a User, and deleting a User will cascade delete their tasks
                entity.HasOne(t => t.User)                      // A TaskModel has one User
                    .WithMany()                                  // A User can have many tasks
                    .HasForeignKey(t => t.UserID)                // Foreign key is UserID
                    .OnDelete(DeleteBehavior.Cascade);           // On delete of a User, all associated Tasks are deleted
            });
        }
    }
}
