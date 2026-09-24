using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Enrollments;

public class CreateModel : PageModel
{
    private readonly StudentRecordApiService _apiService;

    [BindProperty]
    public Enrollment Enrollment { get; set; } = new();

    public List<Student> Students { get; set; } = [];
    public List<Course> Courses { get; set; } = [];
    public List<Semester> Semesters { get; set; } = [];
    public List<SchoolYear> SchoolYears { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public CreateModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        try
        {
            Students = await _apiService.GetStudentsAsync();
            Courses = await _apiService.GetCoursesAsync();
            Semesters = await _apiService.GetSemestersAsync();
            SchoolYears = await _apiService.GetSchoolYearsAsync();
            Enrollment.EnrollmentDate = DateTime.Today;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        try
        {
            await _apiService.CreateEnrollmentAsync(Enrollment);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating enrollment: {ex.Message}";
            await OnGetAsync();
            return Page();
        }
    }
}
