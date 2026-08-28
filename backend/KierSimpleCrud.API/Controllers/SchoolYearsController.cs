using KierSimpleCrud.API.Data;
using KierSimpleCrud.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KierSimpleCrud.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolYearsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public SchoolYearsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolYear>>> GetSchoolYears()
    {
        return Ok(await _dbContext.SchoolYears.OrderByDescending(schoolYear => schoolYear.Sycode).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchoolYear(SchoolYear schoolYear)
    {
        schoolYear.Sycode = schoolYear.Sycode.Trim();
        schoolYear.SchoolYearName = schoolYear.SchoolYearName.Trim();

        if (string.IsNullOrWhiteSpace(schoolYear.Sycode) || string.IsNullOrWhiteSpace(schoolYear.SchoolYearName))
        {
            return BadRequest("School year code and name are required.");
        }

        if (await _dbContext.SchoolYears.AnyAsync(existing => existing.Sycode == schoolYear.Sycode))
        {
            return Conflict("School year code already exists.");
        }

        _dbContext.SchoolYears.Add(schoolYear);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSchoolYears), schoolYear);
    }

    [HttpPut("{sycode}")]
    public async Task<IActionResult> UpdateSchoolYear(string sycode, SchoolYear schoolYear)
    {
        var existingSchoolYear = await _dbContext.SchoolYears.FindAsync(sycode);

        if (existingSchoolYear is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(schoolYear.SchoolYearName))
        {
            return BadRequest("School year is required.");
        }

        existingSchoolYear.SchoolYearName = schoolYear.SchoolYearName.Trim();
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{sycode}")]
    public async Task<IActionResult> DeleteSchoolYear(string sycode)
    {
        var schoolYear = await _dbContext.SchoolYears.FindAsync(sycode);

        if (schoolYear is null)
        {
            return NotFound();
        }

        if (await _dbContext.Enrollments.AnyAsync(enrollment => enrollment.Sycode == sycode))
        {
            return Conflict("School year has enrollment records and cannot be deleted.");
        }

        _dbContext.SchoolYears.Remove(schoolYear);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
