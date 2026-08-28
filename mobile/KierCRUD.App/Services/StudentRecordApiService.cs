using System.Net.Http.Json;
using System.Text.Json;
using KierCRUD.App.Models;

namespace KierCRUD.App.Services;

public class StudentRecordApiService
{
    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    private const string ApiBaseUrl = "http://localhost:5000";

    public Task<List<Student>> GetStudentsAsync(string? search = null)
    {
        return GetListAsync<Student>($"api/students{SearchQuery(search)}");
    }

    public async Task<StudentProfile?> GetStudentAsync(string studid)
    {
        return await _httpClient.GetFromJsonAsync<StudentProfile>($"{ApiBaseUrl}/api/students/{Uri.EscapeDataString(studid)}", _jsonOptions);
    }

    public async Task CreateStudentAsync(Student student)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/api/students", student, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateStudentAsync(Student student)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/api/students/{Uri.EscapeDataString(student.Studid)}", student, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteStudentAsync(string studid)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/api/students/{Uri.EscapeDataString(studid)}");
        await EnsureSuccessAsync(response);
    }

    public Task<List<EnrollmentRead>> GetEnrollmentsAsync(string? search = null)
    {
        return GetListAsync<EnrollmentRead>($"api/enrollments{SearchQuery(search)}");
    }

    public async Task CreateEnrollmentAsync(Enrollment enrollment)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/api/enrollments", enrollment, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateEnrollmentAsync(Enrollment enrollment)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/api/enrollments/{enrollment.EnrollmentId}", enrollment, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteEnrollmentAsync(int enrollmentId)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/api/enrollments/{enrollmentId}");
        await EnsureSuccessAsync(response);
    }

    public Task<List<SchoolYear>> GetSchoolYearsAsync()
    {
        return GetListAsync<SchoolYear>("api/schoolyears");
    }

    public async Task CreateSchoolYearAsync(SchoolYear schoolYear)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/api/schoolyears", schoolYear, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateSchoolYearAsync(SchoolYear schoolYear)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/api/schoolyears/{Uri.EscapeDataString(schoolYear.Sycode)}", schoolYear, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteSchoolYearAsync(string sycode)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/api/schoolyears/{Uri.EscapeDataString(sycode)}");
        await EnsureSuccessAsync(response);
    }

    public Task<List<Course>> GetCoursesAsync()
    {
        return GetListAsync<Course>("api/courses");
    }

    public async Task CreateCourseAsync(Course course)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/api/courses", course, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateCourseAsync(Course course)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/api/courses/{Uri.EscapeDataString(course.Courscode)}", course, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteCourseAsync(string courscode)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/api/courses/{Uri.EscapeDataString(courscode)}");
        await EnsureSuccessAsync(response);
    }

    public Task<List<Semester>> GetSemestersAsync()
    {
        return GetListAsync<Semester>("api/semesters");
    }

    public async Task CreateSemesterAsync(Semester semester)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/api/semesters", semester, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateSemesterAsync(Semester semester)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/api/semesters/{Uri.EscapeDataString(semester.Semcode)}", semester, _jsonOptions);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteSemesterAsync(string semcode)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/api/semesters/{Uri.EscapeDataString(semcode)}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> CheckHealthAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/api/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private async Task<List<T>> GetListAsync<T>(string path)
    {
        var records = await _httpClient.GetFromJsonAsync<List<T>>($"{ApiBaseUrl}/{path}", _jsonOptions);
        return records ?? [];
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var message = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(message) ? response.ReasonPhrase : message);
    }

    private static string SearchQuery(string? search)
    {
        return string.IsNullOrWhiteSpace(search) ? string.Empty : $"?search={Uri.EscapeDataString(search.Trim())}";
    }
}
