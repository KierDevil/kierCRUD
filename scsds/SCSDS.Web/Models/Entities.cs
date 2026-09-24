namespace SCSDS.Web.Models;

public sealed class Student
{
    public int StudentId { get; set; }
    public string StudentNumber { get; set; } = "";
    public string StudentName { get; set; } = "";
    public string Program { get; set; } = "";
    public string YearLevel { get; set; } = "";
    public string Status { get; set; } = "Active";
    public DateTime DateRegistered { get; set; } = DateTime.Today;
}

public sealed class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
}

public class Purchase
{
    public int PurchaseId { get; set; }
    public int StudentId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.Today;
    public decimal Subtotal { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
}

public sealed class PurchaseRow : Purchase
{
    public string StudentName { get; set; } = "";
    public string StudentNumber { get; set; } = "";
    public string ProductName { get; set; } = "";
}

public sealed class SCSDSData
{
    public List<Student> Students { get; set; } = [];
    public List<Product> Products { get; set; } = [];
    public List<Purchase> Purchases { get; set; } = [];
}
