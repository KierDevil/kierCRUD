using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Catalog;

public class IndexModel : PageModel
{
    private readonly StudentRecordApiService _apiService;

    public string? ActiveTab { get; set; } = "SchoolYears";
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    // School Years
    public List<SchoolYear> SchoolYears { get; set; } = [];
    public SchoolYear NewSchoolYear { get; set; } = new();

    // Courses
    public List<Course> Courses { get; set; } = [];
    public Course NewCourse { get; set; } = new();

    // Semesters
    public List<Semester> Semesters { get; set; } = [];
    public Semester NewSemester { get; set; } = new();

    public IndexModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync(string tab = "SchoolYears")
    {
        ActiveTab = tab;
        await LoadAllDataAsync();
    }

    // School Years
    public async Task<IActionResult> OnPostCreateSchoolYearAsync()
    {
        if (!ModelState.IsValid)
        {
            ActiveTab = "SchoolYears";
            await LoadAllDataAsync();
            return Page();
        }

        try
        {
            await _apiService.CreateSchoolYearAsync(NewSchoolYear);
            SuccessMessage = "School Year created successfully!";
            NewSchoolYear = new();
            ActiveTab = "SchoolYears";
            await LoadAllDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            ActiveTab = "SchoolYears";
            await LoadAllDataAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteSchoolYearAsync(string sycode)
    {
        try
        {
            await _apiService.DeleteSchoolYearAsync(sycode);
            ActiveTab = "SchoolYears";
            await LoadAllDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            ActiveTab = "SchoolYears";
            await LoadAllDataAsync();
        }

        return Page();
    }

    // Courses
    public async Task<IActionResult> OnPostCreateCourseAsync()
    {
        if (!ModelState.IsValid)
        {
            ActiveTab = "Courses";
            await LoadAllDataAsync();
            return Page();
        }

        try
        {
            await _apiService.CreateCourseAsync(NewCourse);
            SuccessMessage = "Course created successfully!";
            NewCourse = new();
            ActiveTab = "Courses";
            await LoadAllDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            ActiveTab = "Courses";
            await LoadAllDataAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteCourseAsync(string courscode)
    {
        try
        {
            await _apiService.DeleteCourseAsync(courscode);
            ActiveTab = "Courses";
            await LoadAllDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            ActiveTab = "Courses";
            await LoadAllDataAsync();
        }

        return Page();
    }

    // Semesters
    public async Task<IActionResult> OnPostCreateSemesterAsync()
    {
        if (!ModelState.IsValid)
        {
            ActiveTab = "Semesters";
            await LoadAllDataAsync();
            return Page();
        }

        try
        {
            await _apiService.CreateSemesterAsync(NewSemester);
            SuccessMessage = "Semester created successfully!";
            NewSemester = new();
            ActiveTab = "Semesters";
            await LoadAllDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            ActiveTab = "Semesters";
            await LoadAllDataAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteSemesterAsync(string semcode)
    {
        try
        {
            await _apiService.DeleteSemesterAsync(semcode);
            ActiveTab = "Semesters";
            await LoadAllDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            ActiveTab = "Semesters";
            await LoadAllDataAsync();
        }

        return Page();
    }

    private async Task LoadAllDataAsync()
    {
        try
        {
            SchoolYears = await _apiService.GetSchoolYearsAsync();
            Courses = await _apiService.GetCoursesAsync();
            Semesters = await _apiService.GetSemestersAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
        }
    }
}

