namespace KierCRUD.App.Models;

public class Student
{
    public string Studid { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string Status { get; set; } = "Active";

    public string DisplayName => $"{Studid} - {StudentName}";
}

public class StudentProfile : Student
{
    public List<EnrollmentRead> EnrollmentHistory { get; set; } = [];
}

public class SchoolYear
{
    public string Sycode { get; set; } = string.Empty;

    public string SchoolYearName { get; set; } = string.Empty;

    public string DisplayName => SchoolYearName;
}

public class Course
{
    public string Courscode { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public string DisplayName => $"{Courscode} - {CourseName}";
}

public class Semester
{
    public string Semcode { get; set; } = string.Empty;

    public string SemesterName { get; set; } = string.Empty;

    public string DisplayName => SemesterName;
}

public class Enrollment
{
    public int EnrollmentId { get; set; }

    public string Studid { get; set; } = string.Empty;

    public string Sycode { get; set; } = string.Empty;

    public string Courscode { get; set; } = string.Empty;

    public string Semcode { get; set; } = string.Empty;

    public string Status { get; set; } = "Enrolled";

    public DateTime EnrollmentDate { get; set; } = DateTime.Today;
}

public class EnrollmentRead : Enrollment
{
    public string StudentName { get; set; } = string.Empty;

    public string SchoolYear { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public string SemesterName { get; set; } = string.Empty;

    public string StudentDisplay => $"{Studid} - {StudentName}";

    public string CourseDisplay => $"{Courscode} - {CourseName}";
}
