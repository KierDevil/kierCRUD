using Microsoft.AspNetCore.Mvc.RazorPages;using Library.Web.Services;
namespace Library.Web.Pages;
public sealed class IndexModel:PageModel{private readonly LibraryStore _store;public int Books{get;private set;}public int Members{get;private set;}public int ActiveLoans{get;private set;}public int AvailableCopies{get;private set;}public IndexModel(LibraryStore store)=>_store=store;public void OnGet(){Books=_store.Books.Count;Members=_store.Members.Count;ActiveLoans=_store.Loans.Count(x=>x.ReturnedDate is null);AvailableCopies=_store.Books.Sum(x=>x.AvailableCopies);}}
