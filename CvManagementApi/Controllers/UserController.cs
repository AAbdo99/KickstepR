/* Denne klassen håndterer brukere og tilgang 
- Admin kan opprette brukere (CreateUser)
- Admin kan gi tilgang til andres CV-er (GrantAccess)*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("create-user")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var newUser = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            Role = request.Role
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded) return BadRequest (result.Errors);
        return Ok(new { Message = $"User {request.UserName} created successfully!" });
    }
}

// Data transfer Objects(DTOs)
public class CreateUserRequest
{
    public string UserName { get; set;} = string.Empty;
    public string Email { get; set;} = string.Empty;
    public string Password { get; set;} = string.Empty;

    public UserRole Role { get; set;}
}