namespace Library.Web.Models;

public sealed class Book { public int BookId { get; set; } public string Isbn { get; set; } = ""; public string AccessionNumber { get; set; } = ""; public string BookType { get; set; } = "General"; public string Title { get; set; } = ""; public string Author { get; set; } = ""; public string Category { get; set; } = ""; public int TotalCopies { get; set; } public int AvailableCopies { get; set; } }
public sealed class Member { public int MemberId { get; set; } public string MemberNumber { get; set; } = ""; public string FullName { get; set; } = ""; public string Course { get; set; } = ""; public string YearLevel { get; set; } = ""; public string Status { get; set; } = "Active"; }
public class Loan { public int LoanId { get; set; } public int BookId { get; set; } public int MemberId { get; set; } public string LoanType { get; set; } = "Regular"; public string AccessionNumber { get; set; } = ""; public DateTime BorrowedDate { get; set; } = DateTime.Today; public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14); public DateTime? ReturnedDate { get; set; } public int PenaltyDays { get; set; } public decimal PenaltyAmount { get; set; } }
public sealed class LoanRow : Loan { public string BookTitle { get; set; } = ""; public string BookType { get; set; } = ""; public string MemberName { get; set; } = ""; }
public sealed class LoanReference { public string LoanType { get; set; } = "Regular"; public int DaysAllowed { get; set; } = 14; }
public sealed class PenaltyRate { public string BookType { get; set; } = "General"; public decimal RatePerDay { get; set; } = 10m; }
public sealed class LibraryData { public List<Book> Books { get; set; } = []; public List<Member> Members { get; set; } = []; public List<Loan> Loans { get; set; } = []; }
