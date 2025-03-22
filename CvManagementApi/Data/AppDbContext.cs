using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics; // Legg til denne!
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CV> CVs { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Reference> References { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning)); // 🔹 Ignorer feilen
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CV>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Skill>()
            .HasOne(s => s.CV)
            .WithMany(c => c.Skills)
            .HasForeignKey(s => s.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Education>()
            .HasOne(e => e.CV)
            .WithMany(c => c.Educations)
            .HasForeignKey(e => e.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Experience>()
            .HasOne(exp => exp.CV)
            .WithMany(c => c.Experiences)
            .HasForeignKey(exp => exp.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Reference>()
            .HasOne(r => r.CV)
            .WithMany(c => c.References)
            .HasForeignKey(r => r.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 TESTDATA (HARDCODED, INGEN GUID ELLER DATOER)
        var testUser = new User
        {
            Id = "test-user-1",
            UserName = "testuser",
            Email = "testuser@example.com",
            Role = UserRole.User
        };

        var adminUser = new User
        {
            Id = "admin-user-1",
            UserName = "admin",
            Email = "admin@example.com",
            Role = UserRole.Admin
        };

        var testCV = new CV
        {
            Id = -1,  // 🔹 Endret fra 1 til -1 for å unngå kollisjon
            UserId = testUser.Id,
            FirstName = "Test",
            LastName = "Bruker",
            Email = "testuser@example.com",
            Phone = 12345678
        };

        var adminCV = new CV
        {
            Id = -2,  // 🔹 Endret fra 2 til -2 for å unngå kollisjon
            UserId = adminUser.Id, 
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@example.com",
            Phone = 98765432
        };

        builder.Entity<User>().HasData(testUser, adminUser);
        builder.Entity<CV>().HasData(testCV, adminCV);
    }
}
