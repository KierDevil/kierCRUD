using KierSimpleCrud.API.Data;
using KierSimpleCrud.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KierSimpleCrud.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SemestersController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public SemestersController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Semester>>> GetSemesters()
    {
        return Ok(await _dbContext.Semesters.OrderBy(semester => semester.Semcode).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateSemester(Semester semester)
    {
        semester.Semcode = semester.Semcode.Trim();
        semester.SemesterName = semester.SemesterName.Trim();

        if (string.IsNullOrWhiteSpace(semester.Semcode) || string.IsNullOrWhiteSpace(semester.SemesterName))
        {
            return BadRequest("Semester code and name are required.");
        }

        if (await _dbContext.Semesters.AnyAsync(existing => existing.Semcode == semester.Semcode))
        {
            return Conflict("Semester code already exists.");
        }

        _dbContext.Semesters.Add(semester);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSemesters), semester);
    }

    [HttpPut("{semcode}")]
    public async Task<IActionResult> UpdateSemester(string semcode, Semester semester)
    {
        var existingSemester = await _dbContext.Semesters.FindAsync(semcode);

        if (existingSemester is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(semester.SemesterName))
        {
            return BadRequest("Semester name is required.");
        }

        existingSemester.SemesterName = semester.SemesterName.Trim();
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{semcode}")]
    public async Task<IActionResult> DeleteSemester(string semcode)
    {
        var semester = await _dbContext.Semesters.FindAsync(semcode);

        if (semester is null)
        {
            return NotFound();
        }

        if (await _dbContext.Enrollments.AnyAsync(enrollment => enrollment.Semcode == semcode))
        {
            return Conflict("Semester has enrollment records and cannot be deleted.");
        }

        _dbContext.Semesters.Remove(semester);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
