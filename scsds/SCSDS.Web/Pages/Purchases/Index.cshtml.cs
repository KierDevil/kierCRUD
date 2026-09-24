using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SCSDS.Web.Models;
using SCSDS.Web.Services;

namespace SCSDS.Web.Pages.Purchases;

public sealed class IndexModel : PageModel
{
    private readonly SCSDSStore _store;
    public IReadOnlyList<PurchaseRow> Purchases { get; private set; } = [];
    public IReadOnlyList<Student> Students { get; private set; } = [];
    public IReadOnlyList<Product> Products { get; private set; } = [];
    public string? ErrorMessage { get; private set; }
    [BindProperty] public int StudentId { get; set; }
    [BindProperty] public int ProductId { get; set; }
    [BindProperty] public int Quantity { get; set; } = 1;
    [BindProperty] public DateTime PurchaseDate { get; set; } = DateTime.Today;
    public IndexModel(SCSDSStore store) => _store = store;
    public void OnGet() => Load();
    public IActionResult OnPostCreate()
    {
        try { _store.CreatePurchase(StudentId, ProductId, Quantity, PurchaseDate); return RedirectToPage(); }
        catch (Exception ex) { ErrorMessage = ex.Message; Load(); return Page(); }
    }
    private void Load() { Purchases = _store.Purchases; Students = _store.Students; Products = _store.Products; }
}
