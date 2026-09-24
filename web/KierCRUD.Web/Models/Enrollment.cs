namespace KierCRUD.Web.Models;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public string Studid { get; set; } = string.Empty;
    public string Courscode { get; set; } = string.Empty;
    public string Semcode { get; set; } = string.Empty;
    public string Sycode { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public string Status { get; set; } = "Enrolled";
}

public class EnrollmentRead
{
    public int EnrollmentId { get; set; }
    public string Studid { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string Courscode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string Semcode { get; set; } = string.Empty;
    public string SemesterName { get; set; } = string.Empty;
    public string Sycode { get; set; } = string.Empty;
    public string SchoolYearName { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
