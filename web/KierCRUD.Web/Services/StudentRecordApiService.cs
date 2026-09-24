using System.Text.Json;
using KierCRUD.Web.Models;

namespace KierCRUD.Web.Services;

public class StudentRecordApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);
    private readonly string _apiBaseUrl;

    public StudentRecordApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000";
    }

    // Students
    public async Task<List<Student>> GetStudentsAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        return await GetListAsync<Student>($"api/students{SearchQuery(search)}", cancellationToken);
    }

    public async Task<StudentProfile?> GetStudentAsync(string studid)
    {
        return await _httpClient.GetFromJsonAsync<StudentProfile>(
            $"{_apiBaseUrl}/api/students/{Uri.EscapeDataString(studid)}", 
            _jsonOptions);
    }

    public async Task CreateStudentAsync(Student student)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_apiBaseUrl}/api/students", student, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateStudentAsync(Student student)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"{_apiBaseUrl}/api/students/{Uri.EscapeDataString(student.Studid)}", 
            student, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteStudentAsync(string studid)
    {
        var response = await _httpClient.DeleteAsync(
            $"{_apiBaseUrl}/api/students/{Uri.EscapeDataString(studid)}");
        await EnsureSuccessAsync(response);
    }

    // Enrollments
    public async Task<List<EnrollmentRead>> GetEnrollmentsAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        return await GetListAsync<EnrollmentRead>($"api/enrollments{SearchQuery(search)}", cancellationToken);
    }

    public async Task CreateEnrollmentAsync(Enrollment enrollment)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_apiBaseUrl}/api/enrollments", enrollment, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateEnrollmentAsync(Enrollment enrollment)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"{_apiBaseUrl}/api/enrollments/{enrollment.EnrollmentId}", 
            enrollment, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteEnrollmentAsync(int enrollmentId)
    {
        var response = await _httpClient.DeleteAsync(
            $"{_apiBaseUrl}/api/enrollments/{enrollmentId}");
        await EnsureSuccessAsync(response);
    }

    // School Years
    public async Task<List<SchoolYear>> GetSchoolYearsAsync()
    {
        return await GetListAsync<SchoolYear>("api/schoolyears");
    }

    public async Task CreateSchoolYearAsync(SchoolYear schoolYear)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_apiBaseUrl}/api/schoolyears", schoolYear, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateSchoolYearAsync(SchoolYear schoolYear)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"{_apiBaseUrl}/api/schoolyears/{Uri.EscapeDataString(schoolYear.Sycode)}", 
            schoolYear, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteSchoolYearAsync(string sycode)
    {
        var response = await _httpClient.DeleteAsync(
            $"{_apiBaseUrl}/api/schoolyears/{Uri.EscapeDataString(sycode)}");
        await EnsureSuccessAsync(response);
    }

    // Courses
    public async Task<List<Course>> GetCoursesAsync()
    {
        return await GetListAsync<Course>("api/courses");
    }

    public async Task CreateCourseAsync(Course course)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_apiBaseUrl}/api/courses", course, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateCourseAsync(Course course)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"{_apiBaseUrl}/api/courses/{Uri.EscapeDataString(course.Courscode)}", 
            course, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteCourseAsync(string courscode)
    {
        var response = await _httpClient.DeleteAsync(
            $"{_apiBaseUrl}/api/courses/{Uri.EscapeDataString(courscode)}");
        await EnsureSuccessAsync(response);
    }

    // Semesters
    public async Task<List<Semester>> GetSemestersAsync()
    {
        return await GetListAsync<Semester>("api/semesters");
    }

    public async Task CreateSemesterAsync(Semester semester)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_apiBaseUrl}/api/semesters", semester, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateSemesterAsync(Semester semester)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"{_apiBaseUrl}/api/semesters/{Uri.EscapeDataString(semester.Semcode)}", 
            semester, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteSemesterAsync(string semcode)
    {
        var response = await _httpClient.DeleteAsync(
            $"{_apiBaseUrl}/api/semesters/{Uri.EscapeDataString(semcode)}");
        await EnsureSuccessAsync(response);
    }

    // Health
    public async Task<bool> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/health", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private async Task<List<T>> GetListAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        var records = await _httpClient.GetFromJsonAsync<List<T>>(
            $"{_apiBaseUrl}/{path}", _jsonOptions, cancellationToken);
        return records ?? [];
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var message = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(
            string.IsNullOrWhiteSpace(message) ? response.ReasonPhrase : message);
    }

    private static string SearchQuery(string? search)
    {
        return string.IsNullOrWhiteSpace(search) 
            ? string.Empty 
            : $"?search={Uri.EscapeDataString(search.Trim())}";
    }
}
