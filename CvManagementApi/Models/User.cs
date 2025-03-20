// Brukermodell 

using Microsoft.AspNetCore.Identity;

public enum UserRole
{
    Admin,
    User
}

public class User : IdentityUser
{
    public UserRole Role { get; set; } = UserRole.User;  //Standard er User

    public List<string> AllowedCVs { get; set; } = new(); // Kun Admin brukeren kan gi tilgang til andres CV-er
    
}