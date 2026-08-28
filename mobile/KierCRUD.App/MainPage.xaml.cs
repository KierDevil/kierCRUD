using KierCRUD.App.Models;
using KierCRUD.App.Services;

namespace KierCRUD.App;

public partial class MainPage : ContentPage
{
    private readonly StudentRecordApiService _recordApiService;
    private List<Student> _students = [];
    private List<EnrollmentRead> _enrollments = [];
    private List<SchoolYear> _schoolYears = [];
    private List<Course> _courses = [];
    private List<Semester> _semesters = [];
    private Student? _selectedStudent;
    private EnrollmentRead? _selectedEnrollment;
    private CatalogItem? _selectedCatalogItem;
    private CatalogSection _activeCatalogSection = CatalogSection.SchoolYears;

    public MainPage()
        : this(new StudentRecordApiService())
    {
    }

    public MainPage(StudentRecordApiService recordApiService)
    {
        InitializeComponent();
        _recordApiService = recordApiService;
        StudentStatusPicker.SelectedIndex = 0;
        EnrollmentStatusPicker.SelectedIndex = 0;
        EnrollmentDatePicker.Date = DateTime.Today;
        SetSection(StudentsPanel);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshAllAsync();
    }

    private async Task RefreshAllAsync()
    {
        await CheckConnectionAsync();
        await LoadLookupsAsync();
        await LoadStudentsAsync();
        await LoadEnrollmentsAsync();
        LoadCatalog();
    }

    private async Task CheckConnectionAsync()
    {
        var isOnline = await _recordApiService.CheckHealthAsync();
        StatusLabel.Text = isOnline ? "Connected" : "Backend offline";
        StatusLabel.TextColor = isOnline ? Color.FromArgb("#15803D") : Color.FromArgb("#DC2626");
    }

    private async Task LoadLookupsAsync()
    {
        _students = await _recordApiService.GetStudentsAsync();
        _schoolYears = await _recordApiService.GetSchoolYearsAsync();
        _courses = await _recordApiService.GetCoursesAsync();
        _semesters = await _recordApiService.GetSemestersAsync();

        EnrollmentStudentPicker.ItemsSource = _students;
        EnrollmentSchoolYearPicker.ItemsSource = _schoolYears;
        EnrollmentCoursePicker.ItemsSource = _courses;
        EnrollmentSemesterPicker.ItemsSource = _semesters;
    }

    private async Task LoadStudentsAsync()
    {
        try
        {
            _students = await _recordApiService.GetStudentsAsync(StudentSearchEntry.Text);
            StudentsCollectionView.ItemsSource = _students;
            EnrollmentStudentPicker.ItemsSource = _students;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Load failed", GetErrorMessage(ex), "OK");
        }
    }

    private async Task LoadEnrollmentsAsync()
    {
        try
        {
            _enrollments = await _recordApiService.GetEnrollmentsAsync(EnrollmentSearchEntry.Text);
            EnrollmentsCollectionView.ItemsSource = _enrollments;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Load failed", GetErrorMessage(ex), "OK");
        }
    }

    private void LoadCatalog()
    {
        switch (_activeCatalogSection)
        {
            case CatalogSection.SchoolYears:
                CatalogListTitleLabel.Text = "School Years";
                CatalogFormTitleLabel.Text = _selectedCatalogItem is null ? "Add School Year" : "Edit School Year";
                CatalogCodeEntry.Placeholder = "School year code";
                CatalogNameEntry.Placeholder = "School year";
                CatalogCollectionView.ItemsSource = _schoolYears.Select(item => new CatalogItem(item.Sycode, item.SchoolYearName)).ToList();
                break;
            case CatalogSection.Courses:
                CatalogListTitleLabel.Text = "Courses";
                CatalogFormTitleLabel.Text = _selectedCatalogItem is null ? "Add Course" : "Edit Course";
                CatalogCodeEntry.Placeholder = "Course code";
                CatalogNameEntry.Placeholder = "Course name";
                CatalogCollectionView.ItemsSource = _courses.Select(item => new CatalogItem(item.Courscode, item.CourseName)).ToList();
                break;
            case CatalogSection.Semesters:
                CatalogListTitleLabel.Text = "Semesters";
                CatalogFormTitleLabel.Text = _selectedCatalogItem is null ? "Add Semester" : "Edit Semester";
                CatalogCodeEntry.Placeholder = "Semester code";
                CatalogNameEntry.Placeholder = "Semester name";
                CatalogCollectionView.ItemsSource = _semesters.Select(item => new CatalogItem(item.Semcode, item.SemesterName)).ToList();
                break;
        }
    }

    private void OnStudentsNavClicked(object sender, EventArgs e)
    {
        SetSection(StudentsPanel);
    }

    private void OnEnrollmentsNavClicked(object sender, EventArgs e)
    {
        SetSection(EnrollmentsPanel);
    }

    private void OnSchoolYearsNavClicked(object sender, EventArgs e)
    {
        _activeCatalogSection = CatalogSection.SchoolYears;
        ClearCatalogForm();
        SetSection(CatalogPanel);
        LoadCatalog();
    }

    private void OnCoursesNavClicked(object sender, EventArgs e)
    {
        _activeCatalogSection = CatalogSection.Courses;
        ClearCatalogForm();
        SetSection(CatalogPanel);
        LoadCatalog();
    }

    private void OnSemestersNavClicked(object sender, EventArgs e)
    {
        _activeCatalogSection = CatalogSection.Semesters;
        ClearCatalogForm();
        SetSection(CatalogPanel);
        LoadCatalog();
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await RefreshAllAsync();
    }

    private async void OnStudentSearchChanged(object sender, TextChangedEventArgs e)
    {
        await LoadStudentsAsync();
    }

    private async void OnEnrollmentSearchChanged(object sender, TextChangedEventArgs e)
    {
        await LoadEnrollmentsAsync();
    }

    private async void OnSaveStudentClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(StudentIdEntry.Text) || string.IsNullOrWhiteSpace(StudentNameEntry.Text))
        {
            await DisplayAlert("Missing student", "Enter the student ID and name.", "OK");
            return;
        }

        var student = new Student
        {
            Studid = StudentIdEntry.Text.Trim(),
            StudentName = StudentNameEntry.Text.Trim(),
            Status = StudentStatusPicker.SelectedItem?.ToString() ?? "Active"
        };

        try
        {
            if (_selectedStudent is null)
            {
                await _recordApiService.CreateStudentAsync(student);
            }
            else
            {
                student.Studid = _selectedStudent.Studid;
                await _recordApiService.UpdateStudentAsync(student);
            }

            ClearStudentForm();
            await RefreshAllAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Save failed", GetErrorMessage(ex), "OK");
        }
    }

    private void OnClearStudentClicked(object sender, EventArgs e)
    {
        ClearStudentForm();
    }

    private async void OnViewStudentClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: Student student })
        {
            return;
        }

        try
        {
            var profile = await _recordApiService.GetStudentAsync(student.Studid);
            StudentProfileTitleLabel.Text = profile is null
                ? "Student profile"
                : $"{profile.Studid} - {profile.StudentName} ({profile.Status})";
            StudentHistoryCollectionView.ItemsSource = profile?.EnrollmentHistory ?? [];
        }
        catch (Exception ex)
        {
            await DisplayAlert("Profile failed", GetErrorMessage(ex), "OK");
        }
    }

    private void OnEditStudentClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: Student student })
        {
            return;
        }

        _selectedStudent = student;
        StudentFormTitleLabel.Text = "Edit Student";
        SaveStudentButton.Text = "Update";
        StudentIdEntry.Text = student.Studid;
        StudentIdEntry.IsEnabled = false;
        StudentNameEntry.Text = student.StudentName;
        StudentStatusPicker.SelectedItem = student.Status;
    }

    private async void OnDeleteStudentClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: Student student })
        {
            return;
        }

        var confirmed = await DisplayAlert("Delete student", $"Delete {student.StudentName}?", "Delete", "Cancel");

        if (!confirmed)
        {
            return;
        }

        try
        {
            await _recordApiService.DeleteStudentAsync(student.Studid);
            ClearStudentForm();
            await RefreshAllAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Delete failed", GetErrorMessage(ex), "OK");
        }
    }

    private async void OnSaveEnrollmentClicked(object sender, EventArgs e)
    {
        if (EnrollmentStudentPicker.SelectedItem is not Student student ||
            EnrollmentSchoolYearPicker.SelectedItem is not SchoolYear schoolYear ||
            EnrollmentCoursePicker.SelectedItem is not Course course ||
            EnrollmentSemesterPicker.SelectedItem is not Semester semester)
        {
            await DisplayAlert("Missing enrollment", "Select the student, school year, course, and semester.", "OK");
            return;
        }

        var enrollment = new Enrollment
        {
            EnrollmentId = _selectedEnrollment?.EnrollmentId ?? 0,
            Studid = student.Studid,
            Sycode = schoolYear.Sycode,
            Courscode = course.Courscode,
            Semcode = semester.Semcode,
            Status = EnrollmentStatusPicker.SelectedItem?.ToString() ?? "Enrolled",
            EnrollmentDate = EnrollmentDatePicker.Date
        };

        try
        {
            if (_selectedEnrollment is null)
            {
                await _recordApiService.CreateEnrollmentAsync(enrollment);
            }
            else
            {
                await _recordApiService.UpdateEnrollmentAsync(enrollment);
            }

            ClearEnrollmentForm();
            await RefreshAllAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Save failed", GetErrorMessage(ex), "OK");
        }
    }

    private void OnClearEnrollmentClicked(object sender, EventArgs e)
    {
        ClearEnrollmentForm();
    }

    private void OnEditEnrollmentClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: EnrollmentRead enrollment })
        {
            return;
        }

        _selectedEnrollment = enrollment;
        EnrollmentFormTitleLabel.Text = "Edit Enrollment";
        SaveEnrollmentButton.Text = "Update";
        EnrollmentStudentPicker.SelectedItem = _students.FirstOrDefault(student => student.Studid == enrollment.Studid);
        EnrollmentSchoolYearPicker.SelectedItem = _schoolYears.FirstOrDefault(schoolYear => schoolYear.Sycode == enrollment.Sycode);
        EnrollmentCoursePicker.SelectedItem = _courses.FirstOrDefault(course => course.Courscode == enrollment.Courscode);
        EnrollmentSemesterPicker.SelectedItem = _semesters.FirstOrDefault(semester => semester.Semcode == enrollment.Semcode);
        EnrollmentStatusPicker.SelectedItem = enrollment.Status;
        EnrollmentDatePicker.Date = enrollment.EnrollmentDate.Date;
    }

    private async void OnDeleteEnrollmentClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: EnrollmentRead enrollment })
        {
            return;
        }

        var confirmed = await DisplayAlert("Delete enrollment", $"Delete {enrollment.StudentDisplay} enrollment?", "Delete", "Cancel");

        if (!confirmed)
        {
            return;
        }

        try
        {
            await _recordApiService.DeleteEnrollmentAsync(enrollment.EnrollmentId);
            ClearEnrollmentForm();
            await RefreshAllAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Delete failed", GetErrorMessage(ex), "OK");
        }
    }

    private async void OnSaveCatalogClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CatalogCodeEntry.Text) || string.IsNullOrWhiteSpace(CatalogNameEntry.Text))
        {
            await DisplayAlert("Missing item", "Enter the code and name.", "OK");
            return;
        }

        var code = CatalogCodeEntry.Text.Trim();
        var name = CatalogNameEntry.Text.Trim();

        try
        {
            switch (_activeCatalogSection)
            {
                case CatalogSection.SchoolYears:
                    var schoolYear = new SchoolYear { Sycode = _selectedCatalogItem?.Code ?? code, SchoolYearName = name };
                    if (_selectedCatalogItem is null)
                    {
                        schoolYear.Sycode = code;
                        await _recordApiService.CreateSchoolYearAsync(schoolYear);
                    }
                    else
                    {
                        await _recordApiService.UpdateSchoolYearAsync(schoolYear);
                    }

                    break;
                case CatalogSection.Courses:
                    var course = new Course { Courscode = _selectedCatalogItem?.Code ?? code, CourseName = name };
                    if (_selectedCatalogItem is null)
                    {
                        course.Courscode = code;
                        await _recordApiService.CreateCourseAsync(course);
                    }
                    else
                    {
                        await _recordApiService.UpdateCourseAsync(course);
                    }

                    break;
                case CatalogSection.Semesters:
                    var semester = new Semester { Semcode = _selectedCatalogItem?.Code ?? code, SemesterName = name };
                    if (_selectedCatalogItem is null)
                    {
                        semester.Semcode = code;
                        await _recordApiService.CreateSemesterAsync(semester);
                    }
                    else
                    {
                        await _recordApiService.UpdateSemesterAsync(semester);
                    }

                    break;
            }

            ClearCatalogForm();
            await LoadLookupsAsync();
            LoadCatalog();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Save failed", GetErrorMessage(ex), "OK");
        }
    }

    private void OnClearCatalogClicked(object sender, EventArgs e)
    {
        ClearCatalogForm();
    }

    private void OnEditCatalogClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: CatalogItem item })
        {
            return;
        }

        _selectedCatalogItem = item;
        CatalogCodeEntry.Text = item.Code;
        CatalogCodeEntry.IsEnabled = false;
        CatalogNameEntry.Text = item.Name;
        SaveCatalogButton.Text = "Update";
        CatalogFormTitleLabel.Text = $"Edit {CatalogTitle()}";
    }

    private async void OnDeleteCatalogClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: CatalogItem item })
        {
            return;
        }

        var confirmed = await DisplayAlert($"Delete {CatalogTitle().ToLowerInvariant()}", $"Delete {item.Code}?", "Delete", "Cancel");

        if (!confirmed)
        {
            return;
        }

        try
        {
            switch (_activeCatalogSection)
            {
                case CatalogSection.SchoolYears:
                    await _recordApiService.DeleteSchoolYearAsync(item.Code);
                    break;
                case CatalogSection.Courses:
                    await _recordApiService.DeleteCourseAsync(item.Code);
                    break;
                case CatalogSection.Semesters:
                    await _recordApiService.DeleteSemesterAsync(item.Code);
                    break;
            }

            ClearCatalogForm();
            await LoadLookupsAsync();
            LoadCatalog();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Delete failed", GetErrorMessage(ex), "OK");
        }
    }

    private void SetSection(View activePanel)
    {
        StudentsPanel.IsVisible = activePanel == StudentsPanel;
        EnrollmentsPanel.IsVisible = activePanel == EnrollmentsPanel;
        CatalogPanel.IsVisible = activePanel == CatalogPanel;
    }

    private void ClearStudentForm()
    {
        _selectedStudent = null;
        StudentFormTitleLabel.Text = "Add Student";
        SaveStudentButton.Text = "Create";
        StudentIdEntry.IsEnabled = true;
        StudentIdEntry.Text = string.Empty;
        StudentNameEntry.Text = string.Empty;
        StudentStatusPicker.SelectedIndex = 0;
    }

    private void ClearEnrollmentForm()
    {
        _selectedEnrollment = null;
        EnrollmentFormTitleLabel.Text = "Add Enrollment";
        SaveEnrollmentButton.Text = "Create";
        EnrollmentStudentPicker.SelectedItem = null;
        EnrollmentSchoolYearPicker.SelectedItem = null;
        EnrollmentCoursePicker.SelectedItem = null;
        EnrollmentSemesterPicker.SelectedItem = null;
        EnrollmentStatusPicker.SelectedIndex = 0;
        EnrollmentDatePicker.Date = DateTime.Today;
    }

    private void ClearCatalogForm()
    {
        _selectedCatalogItem = null;
        CatalogFormTitleLabel.Text = $"Add {CatalogTitle()}";
        SaveCatalogButton.Text = "Create";
        CatalogCodeEntry.IsEnabled = true;
        CatalogCodeEntry.Text = string.Empty;
        CatalogNameEntry.Text = string.Empty;
    }

    private string CatalogTitle()
    {
        return _activeCatalogSection switch
        {
            CatalogSection.SchoolYears => "School Year",
            CatalogSection.Courses => "Course",
            CatalogSection.Semesters => "Semester",
            _ => "Item"
        };
    }

    private static string GetErrorMessage(Exception ex)
    {
        return ex.Message;
    }

    private enum CatalogSection
    {
        SchoolYears,
        Courses,
        Semesters
    }

    private record CatalogItem(string Code, string Name);
}
