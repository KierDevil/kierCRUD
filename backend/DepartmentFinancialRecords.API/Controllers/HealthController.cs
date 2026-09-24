using Microsoft.AspNetCore.Mvc;
namespace DepartmentFinancialRecords.API.Controllers;
[ApiController][Route("api/health")] public sealed class HealthController:ControllerBase { [HttpGet] public IActionResult Get()=>Ok(new{status="ok",app="Department Financial Records",time=DateTime.UtcNow}); }
