using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Enrollments;

public class EditModel : PageModel
{
    private readonly StudentRecordApiService _apiService;

    [BindProperty]
    public Enrollment Enrollment { get; set; } = new();

    public List<Student> Students { get; set; } = [];
    public List<Course> Courses { get; set; } = [];
    public List<Semester> Semesters { get; set; } = [];
    public List<SchoolYear> SchoolYears { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public EditModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> OnGetAsync(int enrollmentId)
    {
        await LoadDropdownsAsync();

        try
        {
            var enrollments = await _apiService.GetEnrollmentsAsync();
            var enrollment = enrollments.FirstOrDefault(e => e.EnrollmentId == enrollmentId);
            
            if (enrollment == null)
            {
                return NotFound();
            }

            Enrollment = new Enrollment
            {
                EnrollmentId = enrollment.EnrollmentId,
                Studid = enrollment.Studid,
                Courscode = enrollment.Courscode,
                Semcode = enrollment.Semcode,
                Sycode = enrollment.Sycode,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status
            };
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading enrollment: {ex.Message}";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return Page();
        }

        try
        {
            await _apiService.UpdateEnrollmentAsync(Enrollment);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating enrollment: {ex.Message}";
            await LoadDropdownsAsync();
            return Page();
        }
    }

    private async Task LoadDropdownsAsync()
    {
        try
        {
            Students = await _apiService.GetStudentsAsync();
            Courses = await _apiService.GetCoursesAsync();
            Semesters = await _apiService.GetSemestersAsync();
            SchoolYears = await _apiService.GetSchoolYearsAsync();
        }
        catch
        {
            // Silently fail on dropdown load
        }
    }
}
