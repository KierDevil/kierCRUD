using KierSimpleCrud.API.Data;
using KierSimpleCrud.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KierSimpleCrud.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentRecordsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public StudentRecordsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentRecord>>> GetStudentRecords()
    {
        var records = await _dbContext.StudentRecords
            .OrderByDescending(record => record.Id)
            .ToListAsync();

        return Ok(records);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentRecord>> GetStudentRecord(int id)
    {
        var record = await _dbContext.StudentRecords.FindAsync(id);

        if (record is null)
        {
            return NotFound();
        }

        return Ok(record);
    }

    [HttpPost]
    public async Task<ActionResult<StudentRecord>> CreateStudentRecord(StudentRecord record)
    {
        record.Id = 0;
        record.CreatedAt = DateTime.UtcNow;

        _dbContext.StudentRecords.Add(record);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudentRecord), new { id = record.Id }, record);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStudentRecord(int id, StudentRecord record)
    {
        var existingRecord = await _dbContext.StudentRecords.FindAsync(id);

        if (existingRecord is null)
        {
            return NotFound();
        }

        existingRecord.Name = record.Name;
        existingRecord.Email = record.Email;
        existingRecord.Amount = record.Amount;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudentRecord(int id)
    {
        var record = await _dbContext.StudentRecords.FindAsync(id);

        if (record is null)
        {
            return NotFound();
        }

        _dbContext.StudentRecords.Remove(record);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}

