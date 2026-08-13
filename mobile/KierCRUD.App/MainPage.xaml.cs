using System.Text.RegularExpressions;
using KierCRUD.App.Models;
using KierCRUD.App.Services;

namespace KierCRUD.App;

public partial class MainPage : ContentPage
{
    private readonly StudentRecordApiService _recordApiService;
    private StudentRecord? _selectedRecord;

    public MainPage()
        : this(new StudentRecordApiService())
    {
    }

    public MainPage(StudentRecordApiService recordApiService)
    {
        InitializeComponent();
        _recordApiService = recordApiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckConnectionAsync();
        await LoadRecordsAsync();
    }

    private async Task CheckConnectionAsync()
    {
        var isOnline = await _recordApiService.CheckHealthAsync();
        StatusLabel.Text = isOnline ? "Connected" : "Backend offline";
        StatusLabel.TextColor = isOnline ? Color.FromArgb("#15803D") : Color.FromArgb("#DC2626");
    }

    private async Task LoadRecordsAsync()
    {
        try
        {
            RecordsRefreshView.IsRefreshing = true;
            RecordsCollectionView.ItemsSource = await _recordApiService.GetRecordsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Load failed", ex.Message, "OK");
        }
        finally
        {
            RecordsRefreshView.IsRefreshing = false;
        }
    }

    private async void OnRefreshRecords(object sender, EventArgs e)
    {
        await LoadRecordsAsync();
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await CheckConnectionAsync();
        await LoadRecordsAsync();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Missing name", "Enter the student name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || !IsEmail(EmailEntry.Text))
        {
            await DisplayAlert("Invalid email", "Enter a valid email address.", "OK");
            return;
        }

        if (!decimal.TryParse(AmountEntry.Text, out var amount))
        {
            await DisplayAlert("Invalid amount", "Enter a valid amount.", "OK");
            return;
        }

        var record = new StudentRecord
        {
            Id = _selectedRecord?.Id ?? 0,
            Name = NameEntry.Text.Trim(),
            Email = EmailEntry.Text.Trim(),
            Amount = amount
        };

        try
        {
            if (_selectedRecord is null)
            {
                await _recordApiService.CreateRecordAsync(record);
            }
            else
            {
                await _recordApiService.UpdateRecordAsync(record);
            }

            ClearForm();
            await LoadRecordsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Save failed", ex.Message, "OK");
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: StudentRecord record })
        {
            return;
        }

        _selectedRecord = record;
        FormTitleLabel.Text = "Edit Record";
        SaveButton.Text = "Update";
        NameEntry.Text = record.Name;
        EmailEntry.Text = record.Email;
        AmountEntry.Text = record.Amount.ToString("0.##");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: StudentRecord record })
        {
            return;
        }

        var confirmed = await DisplayAlert("Delete record", $"Delete {record.Name}?", "Delete", "Cancel");

        if (!confirmed)
        {
            return;
        }

        try
        {
            await _recordApiService.DeleteRecordAsync(record.Id);
            await LoadRecordsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Delete failed", ex.Message, "OK");
        }
    }

    private static bool IsEmail(string value)
    {
        return Regex.IsMatch(value.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    private void ClearForm()
    {
        _selectedRecord = null;
        FormTitleLabel.Text = "Add Record";
        SaveButton.Text = "Create";
        NameEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        AmountEntry.Text = string.Empty;
    }
}
