using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Students;

public class IndexModel : PageModel
{
    private readonly StudentRecordApiService _apiService;
    
    [FromQuery(Name = "search")]
    public string? SearchTerm { get; set; }

    [BindProperty]
    public Student NewStudent { get; set; } = new() { Status = "Active" };
    
    public List<Student> Students { get; set; } = [];
    public string? ErrorMessage { get; set; }

    public IndexModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        try
        {
            Students = await _apiService.GetStudentsAsync(SearchTerm);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading students: {ex.Message}";
        }
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (string.IsNullOrWhiteSpace(NewStudent.Studid) || string.IsNullOrWhiteSpace(NewStudent.StudentName))
        {
            ErrorMessage = "Enter the student ID and name.";
            await OnGetAsync();
            return Page();
        }

        try
        {
            await _apiService.CreateStudentAsync(NewStudent);
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating student: {ex.Message}";
            await OnGetAsync();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(string studid)
    {
        try
        {
            await _apiService.DeleteStudentAsync(studid);
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting student: {ex.Message}";
            return Page();
        }
    }
}
