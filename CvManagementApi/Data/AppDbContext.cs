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
        // Ignorer feil, ved småjusteringer. krever ikke migrasjon med engang og praktisk under utvikling. 
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning)); 

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Endre Identity-tabellnavn
        builder.Entity<User>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

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
    }
}
