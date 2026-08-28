using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KierSimpleCrud.API.Models;

public class Semester
{
    [Key]
    [MaxLength(20)]
    public string Semcode { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string SemesterName { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
