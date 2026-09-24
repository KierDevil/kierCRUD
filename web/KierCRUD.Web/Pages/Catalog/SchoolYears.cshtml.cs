using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Catalog;

public class SchoolYearsModel : PageModel
{
    private readonly StudentRecordApiService _apiService;

    [BindProperty]
    public SchoolYear SchoolYear { get; set; } = new();

    public List<SchoolYear> SchoolYears { get; set; } = [];
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public SchoolYearsModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        await LoadSchoolYearsAsync();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadSchoolYearsAsync();
            return Page();
        }

        try
        {
            await _apiService.CreateSchoolYearAsync(SchoolYear);
            SuccessMessage = "School Year created successfully!";
            SchoolYear = new();
            await LoadSchoolYearsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating school year: {ex.Message}";
            await LoadSchoolYearsAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string sycode)
    {
        try
        {
            await _apiService.DeleteSchoolYearAsync(sycode);
            await LoadSchoolYearsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting school year: {ex.Message}";
        }

        return Page();
    }

    private async Task LoadSchoolYearsAsync()
    {
        try
        {
            SchoolYears = await _apiService.GetSchoolYearsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading school years: {ex.Message}";
        }
    }
}
