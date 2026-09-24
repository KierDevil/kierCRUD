using Microsoft.AspNetCore.Mvc.RazorPages;
using SBCCashier.Web.Services;
namespace SBCCashier.Web.Pages;
public sealed class IndexModel : PageModel { private readonly CashierStore _store; public int Students { get; private set; } public int Payments { get; private set; } public int Requests { get; private set; } public decimal TotalCollected { get; private set; } public IndexModel(CashierStore store)=>_store=store; public void OnGet(){Students=_store.Students.Count;Payments=_store.Payments.Count;Requests=_store.RegistrarRequests.Count;TotalCollected=_store.Payments.Sum(x=>x.Amount)+_store.RegistrarRequests.Sum(x=>x.Fee)+_store.VehiclePasses.Sum(x=>x.Amount);} }
