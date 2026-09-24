using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Catalog;

public class SemestersModel : PageModel
{
    private readonly StudentRecordApiService _apiService;

    [BindProperty]
    public Semester Semester { get; set; } = new();

    public List<Semester> Semesters { get; set; } = [];
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public SemestersModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        await LoadSemestersAsync();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadSemestersAsync();
            return Page();
        }

        try
        {
            await _apiService.CreateSemesterAsync(Semester);
            SuccessMessage = "Semester created successfully!";
            Semester = new();
            await LoadSemestersAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating semester: {ex.Message}";
            await LoadSemestersAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string semcode)
    {
        try
        {
            await _apiService.DeleteSemesterAsync(semcode);
            await LoadSemestersAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting semester: {ex.Message}";
        }

        return Page();
    }

    private async Task LoadSemestersAsync()
    {
        try
        {
            Semesters = await _apiService.GetSemestersAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading semesters: {ex.Message}";
        }
    }
}
