namespace KierCRUD.Web.Models;

public class SchoolYear
{
    public string Sycode { get; set; } = string.Empty;
    public string SyName { get; set; } = string.Empty;
}

public class Course
{
    public string Courscode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
}

public class Semester
{
    public string Semcode { get; set; } = string.Empty;
    public string SemesterName { get; set; } = string.Empty;
}
