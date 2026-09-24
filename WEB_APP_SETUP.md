# KierCRUD - Web Version Setup

Your KierCRUD application is now available as a modern web-based application! The web app is built with ASP.NET Core Razor Pages and connects to your existing API backend without any modifications.

## What's New

✅ **Web-Based Interface** - Access your app from any browser  
✅ **Same Features** - All Student, Enrollment, and Catalog management functionality  
✅ **Responsive Design** - Works on desktop and mobile devices  
✅ **Keep Your API** - Uses the existing ASP.NET Core API unchanged  

## Getting Started

### Step 1: Ensure the Backend is Running

Make sure your API backend is running on port 5000:

```powershell
.\run-backend.cmd
```

Or start it manually:
```powershell
cd backend\KierSimpleCrud.API
dotnet run
```

### Step 2: Start the Web Application

**Option A: Start Both Together (Recommended)**
```powershell
.\STARTWEBSES.cmd
```
This opens two windows: one for the backend API and one for the web app.

**Option B: Start Web App Only** (if backend is already running)
```powershell
.\run-web.cmd
```

**Option C: Start from PowerShell**
```powershell
.\run-web.ps1
```

### Step 3: Open in Browser

Once running, open your browser and navigate to:
```
http://localhost:5173
```

You should see the KierCRUD dashboard!

## Web App Features

### 📊 Dashboard
- View summary of all students and enrollments
- Quick access to all management sections
- API health status indicator

### 👥 Students Management
- **View All**: List all students with search functionality
- **Add Student**: Create new student records
- **Edit Student**: Update student information
- **Delete Student**: Remove student records

### 📝 Enrollments Management
- **View All**: List all enrollments with detailed information (student, course, semester, school year)
- **Search**: Filter enrollments by any term
- **Add Enrollment**: Create new course enrollments for students
- **Edit Enrollment**: Modify enrollment details
- **Delete Enrollment**: Remove enrollments

### 📚 Catalog Management
Three sub-sections accessible via tabs:

**School Years**
- Manage academic school years
- Quick add/delete functionality

**Courses**
- Manage available courses
- Full CRUD operations

**Semesters**
- Manage semester definitions
- Tab-based interface

## Project Structure

```
kierCRUD/
├── web/
│   └── KierCRUD.Web/          ← Web application (NEW)
│       ├── Pages/
│       ├── Services/
│       ├── Models/
│       ├── Program.cs
│       ├── appsettings.json
│       └── KierCRUD.Web.csproj
├── backend/
│   └── KierSimpleCrud.API/     ← API backend (unchanged)
├── mobile/
│   └── KierCRUD.App/           ← MAUI desktop app (legacy)
├── STARTWEBSES.cmd             ← Start both backend & web
├── run-web.cmd                 ← Start web app only
└── run-web.ps1                 ← PowerShell version
```

## Configuration

### Change API URL
If your API is running on a different port, edit:
```
web/KierCRUD.Web/appsettings.json
```

Change:
```json
"ApiSettings": {
  "BaseUrl": "http://localhost:5000"
}
```

### Change Web App Port
To run the web app on a different port:
```powershell
cd web\KierCRUD.Web
dotnet run --urls "http://localhost:YOUR_PORT"
```

## Technology Stack

- **ASP.NET Core 8**: Web framework
- **Razor Pages**: Server-side templating
- **Bootstrap 5**: Responsive UI
- **HttpClient**: API communication
- **.NET Entity Framework Core**: (via API)
- **MySQL Database**: Shared with API

## Troubleshooting

### "Cannot connect to API" message
- **Check**: Is the backend running on http://localhost:5000?
- **Fix**: Run `.\run-backend.cmd` in another terminal
- **Edit**: If API is on different port, update `appsettings.json`

### Pages show blank or errors
- **Check**: Is the application running properly? Check the console output
- **Fix**: Close and restart with `.\STARTWEBSES.cmd`

### Database connection errors
- **Check**: Is MySQL running and the `kiercrud` database created?
- **Fix**: Ensure database setup is complete in the backend

### Port 5173 already in use
- **Fix**: Change web app port in the run script or use:
  ```powershell
  dotnet run --urls "http://localhost:DIFFERENT_PORT"
  ```

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

(Any modern browser with JavaScript enabled)

## Next Steps

1. **Start the application** using one of the startup methods above
2. **Create some test data** using the web interface
3. **Explore the features** to ensure everything works
4. **Migrate from desktop app** by decommissioning the MAUI app if desired

## Additional Notes

- The web app uses the same API as your desktop MAUI app
- Both can run simultaneously without conflicts
- Data is stored in the same database
- No changes were made to your existing API or database

## Support

If you encounter issues:
1. Check the browser console (F12) for JavaScript errors
2. Check the application console for .NET errors
3. Verify API is running: `http://localhost:5000/swagger`
4. Verify database connection in API

---

**Happy managing your student records! 📚**
