using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinalProject.Areas.Identity.Data;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace FinalProject.Areas.Identity.Data;

public class FinalProjectIdentityDbContext : IdentityDbContext<User>
{
    public virtual DbSet<Chore> Chores { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ChoreMonth> ChoreMonths { get; set; }

    public FinalProjectIdentityDbContext(DbContextOptions<FinalProjectIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var passwordHasher = new PasswordHasher<User>();

        var user1 = new User
        {
            Id = "8h7cb3u4-8375-0384-d625-nd73hz5h91gw",
            FirstName = "Alex",
            LastName = "Gilmer",
            Email = "alex.gilmer@mitt.ca",
            EmailConfirmed = true,
            UserName = "alex.gilmer@mitt.ca",
            NormalizedEmail = "ALEX.GILMER@MITT.CA",
            NormalizedUserName = "ALEX.GILMER@MITT.CA"
        };

        var user2 = new User
        {
            Id = "hs73mcu2-9264-0276-h827-js82hcbza04h",
            FirstName = "Aayush",
            LastName = "Patel",
            Email = "aayushptl2005@gmail.com",
            EmailConfirmed = true,
            UserName = "aayushptl2005@gmail.com",
            NormalizedEmail = "AAYUSHPTL2005@GMAIL.COM",
            NormalizedUserName = "AAYUSHPTL2005@GMAIL.COM"
        };

        var user3 = new User
        {
            Id = "92n6dhf7-7254-0265-h265-8ch25zmp6hst",
            FirstName = "Chris",
            LastName = "MacDonald",
            Email = "chris.macdonald@mitt.ca",
            EmailConfirmed = true,
            UserName = "chris.macdonald@mitt.ca",
            NormalizedEmail = "CHRIS.MACDONALD@MITT.CA",
            NormalizedUserName = "CHRIS.MACDONALD@MITT.CA"
        };

        user1.PasswordHash = passwordHasher.HashPassword(user1, "Password123");
        user2.PasswordHash = passwordHasher.HashPassword(user2, "Password123");
        user3.PasswordHash = passwordHasher.HashPassword(user3, "Password123");

        builder.Entity<User>().HasData(user1, user2, user3);

        builder.Entity<Category>().HasData(
            new Category { Id = 1, Title = "Cleaning" },
            new Category { Id = 2, Title = "Finance" },
            new Category { Id = 3, Title = "Shopping" },
            new Category { Id = 4, Title = "Groceries" },
            new Category { Id = 5, Title = "Other" }
        );

        builder.Entity<Chore>().HasData(
            new Chore
            {
                Id = 1,
                UserId = user1.Id,
                Name = "Get a Haircut",
                DueDate = new DateTime(2023, 3, 20, 15, 0, 0),
                CategoryId = 5,
                Recurrence = Recurrence.Monthly,
                Completed = false
            },
            new Chore
            {
                Id = 2,
                UserId = user2.Id,
                Name = "Walk the Dog",
                DueDate = new DateTime(2023, 3, 20, 18, 30, 0),
                CategoryId = 5,
                Recurrence = Recurrence.Daily,
                Completed = false
            },
            new Chore
            {
                Id = 3,
                UserId = user3.Id,
                Name = "Buy Groceries",
                DueDate = new DateTime(2023, 3, 25, 20, 0, 0),
                CategoryId = 4,
                Recurrence = Recurrence.Weekly,
                Completed = false
            },
            new Chore
            {
                Id = 4,
                UserId = user1.Id,
                Name = "Dentist Appointment",
                DueDate = new DateTime(2023, 4, 1, 8, 0, 0),
                CategoryId = 5,
                Recurrence = Recurrence.SemiMonthly,
                Completed = false,
            },
            new Chore
            {
                Id = 5,
                Name = "Aayush's Birthday Party",
                DueDate = new DateTime(2023, 6, 30, 18, 0, 0),
                CategoryId = 5,
                Recurrence = Recurrence.Once,
                Completed = false
            },
            new Chore
            {
                Id = 6,
                UserId = user2.Id,
                Name = "Buy New Phone",
                DueDate = new DateTime(2023, 3, 31, 9, 0, 0),
                Recurrence = Recurrence.Once,
                Completed = false
            },
            new Chore
            {
                Id = 7,
                UserId = user1.Id,
                Name = "Do the Laundry",
                DueDate = new DateTime(2023, 3, 29, 12, 0, 0),
                Recurrence = Recurrence.Weekly,
                Completed = false
            }
        );

        builder.Entity<ChoreMonth>().HasData(
            new ChoreMonth(4, "April"),
            new ChoreMonth(4, "October")
        );

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        // Configure primary key for ChoreMonth class
        builder.Entity<ChoreMonth>().HasKey(cm => new { cm.ChoreId, cm.Month });

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);
    }
}