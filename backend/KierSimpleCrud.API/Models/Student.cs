using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KierSimpleCrud.API.Models;

public class Student
{
    [Key]
    [MaxLength(30)]
    public string Studid { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string StudentName { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "Active";

    [JsonIgnore]
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
