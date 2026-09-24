using Microsoft.AspNetCore.Mvc.RazorPages;
using SCSDS.Web.Services;

namespace SCSDS.Web.Pages;

public sealed class IndexModel : PageModel
{
    private readonly SCSDSStore _store;
    public int StudentCount { get; private set; }
    public int ProductCount { get; private set; }
    public int PurchaseCount { get; private set; }
    public decimal SalesTotal { get; private set; }

    public IndexModel(SCSDSStore store) => _store = store;

    public void OnGet()
    {
        StudentCount = _store.Students.Count;
        ProductCount = _store.Products.Count;
        PurchaseCount = _store.Purchases.Count;
        SalesTotal = _store.Purchases.Sum(x => x.TotalAmount);
    }
}
