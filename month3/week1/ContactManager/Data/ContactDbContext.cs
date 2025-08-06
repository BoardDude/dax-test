using Microsoft.EntityFrameworkCore;
using ContactManager.Models;

namespace ContactManager.Data
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Contact entity
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Category).HasConversion<string>();
                
                // Create index on email for faster lookups
                entity.HasIndex(e => e.Email).IsUnique();
                
                // Create index on category for filtering
                entity.HasIndex(e => e.Category);
            });

            // Seed some sample data
            modelBuilder.Entity<Contact>().HasData(
                new Contact
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@email.com",
                    Phone = "555-0101",
                    City = "New York",
                    Category = ContactCategory.Family,
                    Birthday = new DateTime(1985, 3, 15),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Contact
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@company.com",
                    Phone = "555-0202",
                    City = "Los Angeles",
                    Category = ContactCategory.Work,
                    Birthday = new DateTime(1990, 7, 22),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Contact
                {
                    Id = 3,
                    Name = "Bob Wilson",
                    Email = "bob.wilson@email.com",
                    Phone = "555-0303",
                    City = "Chicago",
                    Category = ContactCategory.Friends,
                    Birthday = new DateTime(1988, 11, 8),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
} 