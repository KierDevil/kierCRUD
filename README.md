# Kier CRUD

A small Windows desktop student enrollment CRUD app using the same main technologies as Kier Records, backed by a MySQL database. Now also available as a modern web application!

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- MySQL database
- Swagger / OpenAPI
- .NET MAUI (Desktop)
- ASP.NET Core Razor Pages (Web) ✨ NEW
- Bootstrap 5 (Web)
- Git

## Project Structure

- `backend/KierSimpleCrud.API` - ASP.NET Core backend API
- `web/KierCRUD.Web` - ASP.NET Core Razor Pages web app ✨ NEW
- `mobile/KierCRUD.App` - .NET MAUI Windows desktop app
- `STARTWEBSES.cmd` - starts backend + web app
- `run-web.cmd` - starts web app only ✨ NEW
- `start.cmd` - opens backend and desktop app
- `run-backend.cmd` - starts only the backend
- `run-app.cmd` - starts only the desktop app

## Getting Started - Web App (Recommended)

For the new web-based version, from the project root:

```powershell
.\STARTWEBSES.cmd
```

Then open your browser to:
```
http://localhost:5173
```

See [QUICK_START.md](QUICK_START.md) for a quick guide.  
See [WEB_APP_SETUP.md](WEB_APP_SETUP.md) for detailed setup instructions.

## USB Setup - All Systems

Copy the complete `kierCRUD` folder to a USB drive or another Windows PC. On the other PC, double-click:

```text
setupall.cmd
```

This checks or installs .NET SDK 8 and Node.js LTS when `winget` is available, installs the MAUI Windows workload, restores all .NET projects, installs both frontend dependency sets, builds the Department frontend, and checks MySQL.

The configured MySQL user is `root` with password `higanbana`. MySQL Server must be installed and running on the other PC. The setup script does not delete existing databases. If SQL backups are present in `database-backups`, it offers to restore them.

To copy the current database contents before moving the folder, run:

```text
backup-all-databases.cmd
```

After setup, run `STARTALLSYSTEMS.cmd` to start every web system.

## Getting Started - Desktop App (Legacy)

The API requires MySQL to be running before it starts. It creates the tables and seed data automatically.

Desktop app:

```powershell
cd mobile\KierCRUD.App
dotnet run -f net8.0-windows10.0.19041.0
```

The app connects to `http://localhost:5000` by default. Start the backend first.

Swagger API docs:

```text
http://localhost:5000/swagger
```

`run-backend.cmd` and `run-app.cmd` will use installed `dotnet` by default. If this project is beside your existing `Kier` folder, it can also use `Kier\.dotnet\dotnet.exe`.

## API Endpoints

- `GET /api/students` - list and search students
- `GET /api/students/{studid}` - view a student with enrollment history
- `POST /api/students` - create student
- `PUT /api/students/{studid}` - update student
- `DELETE /api/students/{studid}` - delete student when no enrollments use it
- `GET /api/enrollments` - list and search enrollments with readable joined labels
- `POST /api/enrollments` - create enrollment
- `PUT /api/enrollments/{id}` - update enrollment
- `DELETE /api/enrollments/{id}` - delete enrollment
- `GET /api/schoolyears` - list school years
- `POST /api/schoolyears` - create school year
- `PUT /api/schoolyears/{sycode}` - update school year
- `DELETE /api/schoolyears/{sycode}` - delete school year when no enrollments use it
- `GET /api/courses` - list courses
- `POST /api/courses` - create course
- `PUT /api/courses/{courscode}` - update course
- `DELETE /api/courses/{courscode}` - delete course when no enrollments use it
- `GET /api/semesters` - list semesters
- `POST /api/semesters` - create semester
- `PUT /api/semesters/{semcode}` - update semester
- `DELETE /api/semesters/{semcode}` - delete semester when no enrollments use it
- `GET /api/health` - backend health check

## Push to GitHub

Create a new empty GitHub repository first, then run:

```powershell
git init
git add .
git commit -m "Initial Kier CRUD"
git branch -M main
git remote add origin https://github.com/YOUR_USERNAME/kierCRUD.git
git push -u origin main
```

## Install on Another PC

Open PowerShell, go to Desktop, then clone the project:

```powershell
cd "$env:USERPROFILE\Desktop"
git clone https://github.com/KierDevil/kierCRUD.git
cd kierCRUD
```

Run the setup script:

```powershell
.\setup-windows.cmd
```

If Windows says access is denied, move the `kierCRUD` folder to a user folder such as Desktop or Documents, then run setup again:

```powershell
cd path\to\kierCRUD
.\setup-windows.cmd
```

If shortcut creation is blocked, the app can still be opened from:

```text
publish\KierCRUD\Kier CRUD.vbs
```

After setup, open:

```text
Kier CRUD
```
