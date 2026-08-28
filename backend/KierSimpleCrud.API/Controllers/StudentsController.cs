using KierSimpleCrud.API.Data;
using KierSimpleCrud.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KierSimpleCrud.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public StudentsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentSummaryDto>>> GetStudents([FromQuery] string? search)
    {
        var query = _dbContext.Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(student => student.Studid.Contains(term) || student.StudentName.Contains(term));
        }

        var students = await query
            .OrderBy(student => student.Studid)
            .Select(student => new StudentSummaryDto(student.Studid, student.StudentName, student.Status))
            .ToListAsync();

        return Ok(students);
    }

    [HttpGet("{studid}")]
    public async Task<ActionResult<StudentProfileDto>> GetStudent(string studid)
    {
        var student = await _dbContext.Students
            .Include(student => student.Enrollments)
                .ThenInclude(enrollment => enrollment.SchoolYear)
            .Include(student => student.Enrollments)
                .ThenInclude(enrollment => enrollment.Course)
            .Include(student => student.Enrollments)
                .ThenInclude(enrollment => enrollment.Semester)
            .FirstOrDefaultAsync(student => student.Studid == studid);

        if (student is null)
        {
            return NotFound();
        }

        return Ok(new StudentProfileDto(
            student.Studid,
            student.StudentName,
            student.Status,
            student.Enrollments
                .OrderByDescending(enrollment => enrollment.EnrollmentDate)
                .Select(EnrollmentDtos.FromEntity)
                .ToList()));
    }

    [HttpPost]
    public async Task<ActionResult<StudentSummaryDto>> CreateStudent(Student student)
    {
        student.Studid = student.Studid.Trim();
        student.StudentName = student.StudentName.Trim();
        student.Status = Normalize(student.Status, "Active");

        if (string.IsNullOrWhiteSpace(student.Studid) || string.IsNullOrWhiteSpace(student.StudentName))
        {
            return BadRequest("Student ID and name are required.");
        }

        if (await _dbContext.Students.AnyAsync(existing => existing.Studid == student.Studid))
        {
            return Conflict("Student ID already exists.");
        }

        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { studid = student.Studid }, new StudentSummaryDto(student.Studid, student.StudentName, student.Status));
    }

    [HttpPut("{studid}")]
    public async Task<IActionResult> UpdateStudent(string studid, Student student)
    {
        var existingStudent = await _dbContext.Students.FindAsync(studid);

        if (existingStudent is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(student.StudentName))
        {
            return BadRequest("Student name is required.");
        }

        existingStudent.StudentName = student.StudentName.Trim();
        existingStudent.Status = Normalize(student.Status, "Active");

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{studid}")]
    public async Task<IActionResult> DeleteStudent(string studid)
    {
        var student = await _dbContext.Students.FindAsync(studid);

        if (student is null)
        {
            return NotFound();
        }

        if (await _dbContext.Enrollments.AnyAsync(enrollment => enrollment.Studid == studid))
        {
            return Conflict("Student has enrollment records and cannot be deleted.");
        }

        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private static string Normalize(string? value, string fallback)
    {
        return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
    }
}

public record StudentSummaryDto(string Studid, string StudentName, string Status);

public record StudentProfileDto(string Studid, string StudentName, string Status, List<EnrollmentReadDto> EnrollmentHistory);
