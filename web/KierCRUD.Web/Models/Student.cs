namespace KierCRUD.Web.Models;

public class Student
{
    public string Studid { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
}

public class StudentProfile
{
    public string Studid { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
