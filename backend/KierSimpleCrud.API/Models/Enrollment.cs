using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KierSimpleCrud.API.Models;

public class Enrollment
{
    public int EnrollmentId { get; set; }

    [Required]
    [MaxLength(30)]
    public string Studid { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Sycode { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Courscode { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Semcode { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "Enrolled";

    public DateTime EnrollmentDate { get; set; } = DateTime.Today;

    [JsonIgnore]
    public Student? Student { get; set; }

    [JsonIgnore]
    public SchoolYear? SchoolYear { get; set; }

    [JsonIgnore]
    public Course? Course { get; set; }

    [JsonIgnore]
    public Semester? Semester { get; set; }
}
