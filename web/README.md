# KierCRUD Web Application

This is the new web-based version of KierCRUD, built with ASP.NET Core Razor Pages.

## Project Structure

```
web/KierCRUD.Web/
├── Pages/
│   ├── Index.cshtml           # Dashboard home page
│   ├── Students/              # Student management pages
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   ├── Enrollments/           # Enrollment management pages
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   ├── Catalog/               # Catalog management pages
│   │   ├── Index.cshtml
│   │   ├── SchoolYearsTab.cshtml
│   │   ├── CoursesTab.cshtml
│   │   └── SemestersTab.cshtml
│   └── Shared/                # Shared layouts
├── Services/                  # API service layer
│   └── StudentRecordApiService.cs
├── Models/                    # Data models
├── Program.cs                 # Application entry point
├── appsettings.json          # Configuration
└── KierCRUD.Web.csproj       # Project file
```

## Features

- **Students Management**: Create, read, update, and delete student records
- **Enrollments Management**: Manage course enrollments for students
- **Catalog Management**: Manage school years, courses, and semesters
- **Search**: Filter students and enrollments by search terms
- **Responsive Design**: Bootstrap 5 for mobile-friendly pages

## Requirements

- .NET SDK 8.0 or later
- Running KierCRUD API backend on http://localhost:5000

## How to Run

### Option 1: Start Both Backend and Web App
```powershell
.\STARTWEBSES.cmd
```

This will open two terminal windows:
- Backend API on http://localhost:5000
- Web App on http://localhost:5173

### Option 2: Start Only the Web App
Make sure the backend is running first, then:
```powershell
.\run-web.cmd
```

Or with PowerShell:
```powershell
.\run-web.ps1
```

Or from the web project directory:
```powershell
cd web\KierCRUD.Web
dotnet run --urls "http://localhost:5173"
```

## Configuration

Edit `web/KierCRUD.Web/appsettings.json` to change:
- API base URL (default: `http://localhost:5000`)
- Other application settings

## Browser Access

Once running, open your browser and navigate to:
```
http://localhost:5173
```

## Architecture

The web app uses:
- **ASP.NET Core Razor Pages**: Server-side rendering
- **HttpClient**: Communication with the API
- **Bootstrap 5**: Responsive UI framework
- **Dependency Injection**: Service container for the API service

## API Service

The `StudentRecordApiService` handles all communication with the backend API. It provides methods for:
- Student CRUD operations
- Enrollment CRUD operations
- School Year, Course, and Semester management
- Health check for API connectivity

## Pages Overview

- **Home (/)**: Dashboard with summary of students and enrollments
- **Students (/Students/*)**: Student CRUD operations
- **Enrollments (/Enrollments/*)**: Enrollment CRUD operations
- **Catalog (/Catalog/*)**: Tab-based management of school years, courses, and semesters
