using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Catalog;

public class CoursesModel : PageModel
{
    private readonly StudentRecordApiService _apiService;

    [BindProperty]
    public Course Course { get; set; } = new();

    public List<Course> Courses { get; set; } = [];
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public CoursesModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        await LoadCoursesAsync();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadCoursesAsync();
            return Page();
        }

        try
        {
            await _apiService.CreateCourseAsync(Course);
            SuccessMessage = "Course created successfully!";
            Course = new();
            await LoadCoursesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating course: {ex.Message}";
            await LoadCoursesAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string courscode)
    {
        try
        {
            await _apiService.DeleteCourseAsync(courscode);
            await LoadCoursesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting course: {ex.Message}";
        }

        return Page();
    }

    private async Task LoadCoursesAsync()
    {
        try
        {
            Courses = await _apiService.GetCoursesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading courses: {ex.Message}";
        }
    }
}
