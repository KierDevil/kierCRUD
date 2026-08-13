using Microsoft.AspNetCore.Mvc;

namespace KierSimpleCrud.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            app = "Kier CRUD",
            time = DateTime.UtcNow
        });
    }
}
