using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SCSDS.Web.Models;
using SCSDS.Web.Services;

namespace SCSDS.Web.Pages.Students;

public sealed class IndexModel : PageModel
{
    private readonly SCSDSStore _store;
    public IReadOnlyList<Student> Students { get; private set; } = [];
    public string? ErrorMessage { get; private set; }
    [BindProperty] public Student NewStudent { get; set; } = new() { Status = "Active" };

    public IndexModel(SCSDSStore store) => _store = store;
    public void OnGet() => Load();

    public IActionResult OnPostCreate()
    {
        if (string.IsNullOrWhiteSpace(NewStudent.StudentNumber) || string.IsNullOrWhiteSpace(NewStudent.StudentName))
        {
            ErrorMessage = "Student number and student name are required.";
            Load();
            return Page();
        }
        try { _store.AddStudent(NewStudent); return RedirectToPage(); }
        catch (Exception ex) { ErrorMessage = ex.Message; Load(); return Page(); }
    }

    public IActionResult OnPostDelete(int id)
    {
        try { _store.DeleteStudent(id); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        Load();
        return Page();
    }

    private void Load() => Students = _store.Students;
}
