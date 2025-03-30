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
    public DbSet<Award> Awards { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Presentation> Presentations { get; set; }
    public DbSet<ProjectExperience> ProjectExperiences { get; set; }
    public DbSet<RoleOverview> RoleOverviews { get; set; }

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

        builder.Entity<Award>()
            .HasOne(a => a.CV)
            .WithMany(c => c.Awards)
            .HasForeignKey(a => a.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Certification>()
            .HasOne(c=> c.CV)
            .WithMany(c => c.Certifications)
            .HasForeignKey(c => c.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Course>()
            .HasOne(c=> c.CV)
            .WithMany(c => c.Courses)
            .HasForeignKey(c => c.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Language>()
            .HasOne(l=> l.CV)
            .WithMany(c => c.Languages)
            .HasForeignKey(l => l.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Position>()
            .HasOne(p=> p.CV)
            .WithMany(c => c.Positions)
            .HasForeignKey(p => p.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Presentation>()
            .HasOne(p=> p.CV)
            .WithMany(c => c.Presentations)
            .HasForeignKey(p => p.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProjectExperience>()
            .HasOne(p=> p.CV)
            .WithMany(c => c.ProjectExperiences)
            .HasForeignKey(p => p.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RoleOverview>()
            .HasOne(r=> r.CV)
            .WithMany(c => c.RoleOverviews)
            .HasForeignKey(r => r.CVId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}