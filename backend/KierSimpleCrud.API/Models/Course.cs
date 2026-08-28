using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KierSimpleCrud.API.Models;

public class Course
{
    [Key]
    [MaxLength(20)]
    public string Courscode { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string CourseName { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
