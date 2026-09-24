using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SCSDS.Web.Models;
using SCSDS.Web.Services;

namespace SCSDS.Web.Pages.Products;

public sealed class IndexModel : PageModel
{
    private readonly SCSDSStore _store;
    public IReadOnlyList<Product> Products { get; private set; } = [];
    public string? ErrorMessage { get; private set; }
    [BindProperty] public Product NewProduct { get; set; } = new();
    public IndexModel(SCSDSStore store) => _store = store;
    public void OnGet() => Load();
    public IActionResult OnPostCreate()
    {
        if (string.IsNullOrWhiteSpace(NewProduct.ProductName) || NewProduct.UnitPrice < 0 || NewProduct.StockQuantity < 0) { ErrorMessage = "Enter a product name, price, and non-negative stock."; Load(); return Page(); }
        _store.AddProduct(NewProduct); return RedirectToPage();
    }
    public IActionResult OnPostDelete(int id) { try { _store.DeleteProduct(id); } catch (Exception ex) { ErrorMessage = ex.Message; } Load(); return Page(); }
    private void Load() => Products = _store.Products;
}
