using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;


/* Denne klassen håndterer brukere og tilgang 
- Admin kan opprette brukere (CreateUser)
- Admin kan gi tilgang til andres CV-er (GrantAccess)*/

[ApiController]
[Route("api/user")] // API-ruten blir f.eks. api/user/login, api/user/create-user
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager; // Identity-verktøy for brukere (oppretting, validering, osv.)
    private readonly JwtService _jwtService;         // Hjelpeklasse for å generere JWT-token

    // Dependency Injection av UserManager og JwtService
    public UserController(UserManager<User> userManager, JwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    // LOGIN-API
    // Alle (også ikke-innloggede) kan bruke denne for å logge inn og motta en JWT-token
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Finn bruker basert på brukernavn
        var user = await _userManager.FindByNameAsync(request.Username);

        // Sjekk om bruker finnes og om passordet stemmer
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // ✅ Generer JWT-token hvis alt er ok
        var token = _jwtService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    // REATE USER-API (kun for Admin)
    // Kun brukere med "Admin"-rolle har tilgang til denne
    [HttpPost("create")]
      [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        // Opprett ny bruker basert på request-data
        var newUser = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            Role = request.Role
        };

        // Legg til i databasen med passord
        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors); // Feil? Returner valideringsfeil fra Identity

        return Ok(new { Message = $"User {request.UserName} created successfully!" });
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        return Ok(users);
    }
}

#region DTOs (Data Transfer Objects)

// Brukes til login-endepunktet
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// Brukes til create-user-endepunktet
public class CreateUserRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } // Enum: Admin eller User
}

#endregion
