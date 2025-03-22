using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

/* Denne klassen håndterer CV-er og gir disse tilgangene:
- Brukere kan opprette, redigere, slette sin egen CV
- Admin kan se, redigere, slette alle CV-er.
- Brukere kan kun se andres CV-er hvis de har tilgang */

[ApiController]
[Route("api/[controller]")]
public class CVController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public CVController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    // Opprett en CV - kun for innloggede brukere
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateCV([FromBody] CV cv)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        cv.UserId = user.Id;
        _context.CVs.Add(cv);
        await _context.SaveChangesAsync();

        return Ok(cv);
    }

    // Hent alle CV-er (Admin ser alt, vanlige brukere ser kun sine egne eller de de har tilgang til)
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllCVs()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        IQueryable<CV> query = _context.CVs
            .Include(c => c.Skills)
            .Include(c => c.Educations)
            .Include(c => c.Experiences)
            .Include(c => c.References);

        if (user.Role != UserRole.Admin)
        {
            query = query.Where(c => c.UserId == user.Id);
        }

        var cvs = await query.ToListAsync();
        return Ok(cvs);
    }

    // Hent en CV (Admin eller bruker med tilgang)
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetCV(int id)
    {
        var cv = await _context.CVs.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == id);
        if (cv == null) return NotFound();

        var requestingUser = await _userManager.GetUserAsync(User);
        if (requestingUser == null || (requestingUser.Role != UserRole.Admin && cv.UserId != requestingUser.Id))
        {
            return Forbid();
        }

        return Ok(cv);
    }


    // Oppdater en CV- Kun eieren altså bruker eller Admin
    [HttpPut("update/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateCV(int id, [FromBody] CV updatedCV)
    {
        var cv = await _context.CVs.FindAsync(id);
        if (cv == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (user == null || (user.Role != UserRole.Admin && cv.UserId != user.Id))
        {
            return Forbid(); // kun Admin eller eier kan oppdatere CV-en
        }

        // Oppdaterer CV-feltene
        cv.FirstName = updatedCV.FirstName;
        cv.LastName = updatedCV.LastName;
        cv.Email = updatedCV.Email;
        cv.Phone = updatedCV.Phone;
        cv.Skills = updatedCV.Skills;
        cv.Educations = updatedCV.Educations;
        cv.Experiences = updatedCV.Experiences;
        cv.References = updatedCV.References;

        await _context.SaveChangesAsync();
        return Ok(cv);
    }

    // Slett en CV - tilgjengelig kun for Admin eller eier(bruker)
    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCV(int id)
    {
        var cv = await _context.CVs.FindAsync(id);
        if (cv == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (user == null || (user.Role != UserRole.Admin && cv.UserId != user.Id))
        {
            return Forbid();
        }

        _context.CVs.Remove(cv);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "CV deleted successfully" });
    }
}
