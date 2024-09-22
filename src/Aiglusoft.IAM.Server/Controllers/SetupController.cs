using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aiglusoft.IAM.Server.Controllers
{
  [ApiController]
  [Route("setup")]
  public class SetupController : ControllerBase
  {
    private readonly AppDbContext _context;

    public SetupController(AppDbContext context)
    {
      _context = context;
    }

    // Endpoint pour exécuter les migrations de la base de données
    [HttpPost("")]
    public async Task<IActionResult> Setup()
    {
      try
      {
        // Appliquer les migrations à la base de données
        await _context.Database.MigrateAsync();
        return Ok("Migrations exécutées avec succès.");
      }
      catch (Exception ex)
      {
        // Retourner une erreur si les migrations échouent
        return StatusCode(500, $"Erreur lors de l'exécution des migrations : {ex.Message}");
      }
    }
  }
}