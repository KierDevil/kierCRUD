using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Students;

public class CreateModel : PageModel
{
    private readonly StudentRecordApiService _apiService;

    [BindProperty]
    public Student Student { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public CreateModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await _apiService.CreateStudentAsync(Student);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating student: {ex.Message}";
            return Page();
        }
    }
}
