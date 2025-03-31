// CV-modell med relasjoner! 

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

public class CV
{
    public int Id{ get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public int Phone { get; set; }

    public List<Skill> Skills{ get; set; } = new();
    public List <Education> Educations { get; set; } = new();
    public List<Experience> Experiences { get; set; } = new();
    public List <Reference> References{ get; set; } = new();
    public List <Award> Awards{ get; set; } = new();
    public List <Certification> Certifications{ get; set; } = new();
    public List <Course> Courses { get; set; } = new();
    public List <Language> Languages { get; set; } = new();
    public List <Position> Positions { get; set; } = new();
    public List <Presentation> Presentations { get; set; } = new();
    public List <ProjectExperience> ProjectExperiences { get; set; } = new();
    public List <RoleOverview> RoleOverviews { get; set; } = new();

    

    public bool CanView(User requestingUser)
{
    return requestingUser.Role == UserRole.Admin 
        || requestingUser.Id == UserId 
        || (User?.AllowedCVs.Contains(UserId) ?? false); // Sikrer å sjekke at AllowedCVs sjkkes, dersom User ikke er null
}



}