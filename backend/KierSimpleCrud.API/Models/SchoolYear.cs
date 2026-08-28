using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KierSimpleCrud.API.Models;

public class SchoolYear
{
    [Key]
    [MaxLength(20)]
    public string Sycode { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string SchoolYearName { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
