using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KierCRUD.Web.Models;
using KierCRUD.Web.Services;

namespace KierCRUD.Web.Pages.Students;

public class EditModel : PageModel
{
    private readonly StudentRecordApiService _apiService;

    [BindProperty]
    public Student Student { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public EditModel(StudentRecordApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> OnGetAsync(string studid)
    {
        if (string.IsNullOrEmpty(studid))
        {
            return NotFound();
        }

        try
        {
            var profile = await _apiService.GetStudentAsync(studid);
            if (profile == null)
            {
                return NotFound();
            }

            Student = new Student
            {
                Studid = profile.Studid,
                StudentName = profile.StudentName,
                Status = profile.Status
            };
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading student: {ex.Message}";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await _apiService.UpdateStudentAsync(Student);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating student: {ex.Message}";
            return Page();
        }
    }
}
