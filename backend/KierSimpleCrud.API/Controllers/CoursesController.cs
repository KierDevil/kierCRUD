using KierSimpleCrud.API.Data;
using KierSimpleCrud.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KierSimpleCrud.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public CoursesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        return Ok(await _dbContext.Courses.OrderBy(course => course.Courscode).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(Course course)
    {
        course.Courscode = course.Courscode.Trim();
        course.CourseName = course.CourseName.Trim();

        if (string.IsNullOrWhiteSpace(course.Courscode) || string.IsNullOrWhiteSpace(course.CourseName))
        {
            return BadRequest("Course code and name are required.");
        }

        if (await _dbContext.Courses.AnyAsync(existing => existing.Courscode == course.Courscode))
        {
            return Conflict("Course code already exists.");
        }

        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCourses), course);
    }

    [HttpPut("{courscode}")]
    public async Task<IActionResult> UpdateCourse(string courscode, Course course)
    {
        var existingCourse = await _dbContext.Courses.FindAsync(courscode);

        if (existingCourse is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(course.CourseName))
        {
            return BadRequest("Course name is required.");
        }

        existingCourse.CourseName = course.CourseName.Trim();
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{courscode}")]
    public async Task<IActionResult> DeleteCourse(string courscode)
    {
        var course = await _dbContext.Courses.FindAsync(courscode);

        if (course is null)
        {
            return NotFound();
        }

        if (await _dbContext.Enrollments.AnyAsync(enrollment => enrollment.Courscode == courscode))
        {
            return Conflict("Course has enrollment records and cannot be deleted.");
        }

        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
