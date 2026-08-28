# Kier CRUD

A small Windows desktop student enrollment CRUD app using the same main technologies as Kier Records, backed by a MySQL database.

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- MySQL database
- Swagger / OpenAPI
- .NET MAUI
- Git

## Project Structure

- `backend/KierSimpleCrud.API` - ASP.NET Core backend API
- `mobile/KierCRUD.App` - .NET MAUI Windows desktop app
- `start.cmd` - opens backend and desktop app
- `setup-windows.cmd` - one-file setup for another Windows PC
- `publish-windows.cmd` - creates a click-to-run Windows folder
- `create-shortcuts.cmd` - adds Desktop and Start Menu shortcuts
- `run-backend.cmd` - starts only the backend
- `run-app.cmd` - starts only the desktop app

## Requirements

Install these on the other PC:

- .NET SDK 8 x64, only needed to build from source
- Git

## Database Setup

Install MySQL, make sure it is running on `localhost:3306`, and create the `kiercrud` database. The default connection uses user `root` and password `higanbana`.

For security, set `DB_CONNECTION_STRING` instead of storing credentials in `appsettings.json` when sharing or deploying the API.

## How to Run

From the project root:

```powershell
.\start.cmd
```

This starts visible backend and app launcher windows so you can see what is happening while the app runs. To stop, press `Ctrl+C` or close the terminals.

Or run manually in separate terminals.

Backend:

```powershell
cd backend\KierSimpleCrud.API
dotnet restore
dotnet run --urls http://localhost:5000
```

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
