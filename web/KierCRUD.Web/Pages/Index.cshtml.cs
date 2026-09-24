using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KierCRUD.Web.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("/Students/Index");
    }
}
