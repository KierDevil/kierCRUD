using KierSimpleCrud.API.Data;
using KierSimpleCrud.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KierSimpleCrud.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public EnrollmentsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnrollmentReadDto>>> GetEnrollments([FromQuery] string? search)
    {
        var query = EnrollmentQuery();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(enrollment =>
                enrollment.Studid.Contains(term) ||
                enrollment.Student!.StudentName.Contains(term) ||
                enrollment.SchoolYear!.SchoolYearName.Contains(term) ||
                enrollment.Course!.Courscode.Contains(term) ||
                enrollment.Course.CourseName.Contains(term) ||
                enrollment.Semester!.SemesterName.Contains(term) ||
                enrollment.Status.Contains(term));
        }

        var enrollments = await query
            .OrderByDescending(enrollment => enrollment.EnrollmentDate)
            .ThenBy(enrollment => enrollment.Studid)
            .ToListAsync();

        return Ok(enrollments.Select(EnrollmentDtos.FromEntity));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EnrollmentReadDto>> GetEnrollment(int id)
    {
        var enrollment = await EnrollmentQuery().FirstOrDefaultAsync(enrollment => enrollment.EnrollmentId == id);

        if (enrollment is null)
        {
            return NotFound();
        }

        return Ok(EnrollmentDtos.FromEntity(enrollment));
    }

    [HttpPost]
    public async Task<ActionResult<EnrollmentReadDto>> CreateEnrollment(Enrollment enrollment)
    {
        enrollment.EnrollmentId = 0;
        var validation = await ValidateEnrollmentAsync(enrollment);

        if (validation is not null)
        {
            return validation;
        }

        if (await HasDuplicateEnrollmentAsync(enrollment))
        {
            return Conflict("Student already has an enrollment for this school year and semester.");
        }

        _dbContext.Enrollments.Add(enrollment);
        await _dbContext.SaveChangesAsync();

        var created = await EnrollmentQuery().FirstAsync(item => item.EnrollmentId == enrollment.EnrollmentId);
        return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.EnrollmentId }, EnrollmentDtos.FromEntity(created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEnrollment(int id, Enrollment enrollment)
    {
        var existingEnrollment = await _dbContext.Enrollments.FindAsync(id);

        if (existingEnrollment is null)
        {
            return NotFound();
        }

        enrollment.EnrollmentId = id;
        var validation = await ValidateEnrollmentAsync(enrollment);

        if (validation is not null)
        {
            return validation;
        }

        if (await HasDuplicateEnrollmentAsync(enrollment, id))
        {
            return Conflict("Student already has an enrollment for this school year and semester.");
        }

        existingEnrollment.Studid = enrollment.Studid.Trim();
        existingEnrollment.Sycode = enrollment.Sycode.Trim();
        existingEnrollment.Courscode = enrollment.Courscode.Trim();
        existingEnrollment.Semcode = enrollment.Semcode.Trim();
        existingEnrollment.Status = Normalize(enrollment.Status, "Enrolled");
        existingEnrollment.EnrollmentDate = enrollment.EnrollmentDate.Date;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEnrollment(int id)
    {
        var enrollment = await _dbContext.Enrollments.FindAsync(id);

        if (enrollment is null)
        {
            return NotFound();
        }

        _dbContext.Enrollments.Remove(enrollment);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private IQueryable<Enrollment> EnrollmentQuery()
    {
        return _dbContext.Enrollments
            .Include(enrollment => enrollment.Student)
            .Include(enrollment => enrollment.SchoolYear)
            .Include(enrollment => enrollment.Course)
            .Include(enrollment => enrollment.Semester);
    }

    private async Task<ActionResult?> ValidateEnrollmentAsync(Enrollment enrollment)
    {
        enrollment.Studid = enrollment.Studid.Trim();
        enrollment.Sycode = enrollment.Sycode.Trim();
        enrollment.Courscode = enrollment.Courscode.Trim();
        enrollment.Semcode = enrollment.Semcode.Trim();
        enrollment.Status = Normalize(enrollment.Status, "Enrolled");
        enrollment.EnrollmentDate = enrollment.EnrollmentDate == default ? DateTime.Today : enrollment.EnrollmentDate.Date;

        if (string.IsNullOrWhiteSpace(enrollment.Studid) ||
            string.IsNullOrWhiteSpace(enrollment.Sycode) ||
            string.IsNullOrWhiteSpace(enrollment.Courscode) ||
            string.IsNullOrWhiteSpace(enrollment.Semcode))
        {
            return BadRequest("Student, school year, course, and semester are required.");
        }

        if (!await _dbContext.Students.AnyAsync(student => student.Studid == enrollment.Studid))
        {
            return BadRequest("Selected student does not exist.");
        }

        if (!await _dbContext.SchoolYears.AnyAsync(schoolYear => schoolYear.Sycode == enrollment.Sycode))
        {
            return BadRequest("Selected school year does not exist.");
        }

        if (!await _dbContext.Courses.AnyAsync(course => course.Courscode == enrollment.Courscode))
        {
            return BadRequest("Selected course does not exist.");
        }

        if (!await _dbContext.Semesters.AnyAsync(semester => semester.Semcode == enrollment.Semcode))
        {
            return BadRequest("Selected semester does not exist.");
        }

        return null;
    }

    private Task<bool> HasDuplicateEnrollmentAsync(Enrollment enrollment, int? exceptEnrollmentId = null)
    {
        return _dbContext.Enrollments.AnyAsync(existing =>
            existing.Studid == enrollment.Studid &&
            existing.Sycode == enrollment.Sycode &&
            existing.Semcode == enrollment.Semcode &&
            existing.EnrollmentId != exceptEnrollmentId);
    }

    private static string Normalize(string? value, string fallback)
    {
        return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
    }
}

public record EnrollmentReadDto(
    int EnrollmentId,
    string Studid,
    string StudentName,
    string Sycode,
    string SchoolYear,
    string Courscode,
    string CourseName,
    string Semcode,
    string SemesterName,
    string Status,
    DateTime EnrollmentDate);

public static class EnrollmentDtos
{
    public static EnrollmentReadDto FromEntity(Enrollment enrollment)
    {
        return new EnrollmentReadDto(
            enrollment.EnrollmentId,
            enrollment.Studid,
            enrollment.Student?.StudentName ?? string.Empty,
            enrollment.Sycode,
            enrollment.SchoolYear?.SchoolYearName ?? string.Empty,
            enrollment.Courscode,
            enrollment.Course?.CourseName ?? string.Empty,
            enrollment.Semcode,
            enrollment.Semester?.SemesterName ?? string.Empty,
            enrollment.Status,
            enrollment.EnrollmentDate);
    }
}
