using System.Net.Http.Json;
using System.Text.Json;
using KierCRUD.App.Models;

namespace KierCRUD.App.Services;

public class StudentRecordApiService
{
    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    private const string ApiBaseUrl = "http://localhost:5000";

    public async Task<List<StudentRecord>> GetRecordsAsync()
    {
        var records = await _httpClient.GetFromJsonAsync<List<StudentRecord>>($"{ApiBaseUrl}/api/studentrecords", _jsonOptions);
        return records ?? [];
    }

    public async Task CreateRecordAsync(StudentRecord record)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/api/studentrecords", record, _jsonOptions);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateRecordAsync(StudentRecord record)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/api/studentrecords/{record.Id}", record, _jsonOptions);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRecordAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/api/studentrecords/{id}");
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
}

