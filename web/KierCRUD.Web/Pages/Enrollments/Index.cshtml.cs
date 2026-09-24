using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Enrollments;

public class IndexModel : PageModel
{
    private readonly StudentRecordApiService _apiService;
    
    [FromQuery(Name = "search")]
    public string? SearchTerm { get; set; }
    
    public List<EnrollmentRead> Enrollments { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public IndexModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        try
        {
            Enrollments = await _apiService.GetEnrollmentsAsync(SearchTerm);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading enrollments: {ex.Message}";
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int enrollmentId)
    {
        try
        {
            await _apiService.DeleteEnrollmentAsync(enrollmentId);
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting enrollment: {ex.Message}";
            return Page();
        }
    }
}
