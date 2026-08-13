# Kier CRUD

A small Windows desktop student-record CRUD app using the same main technologies as Kier Records, but with a built-in local database file so there is no MySQL password or database setup.

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQLite local database
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

No database setup is required.

The backend creates a local SQLite database file automatically:

```text
backend/KierSimpleCrud.API/kiercrud.db
```

This file is ignored by Git because it is local data.

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

- `GET /api/studentrecords` - list records
- `GET /api/studentrecords/{id}` - get one record
- `POST /api/studentrecords` - create record
- `PUT /api/studentrecords/{id}` - update record
- `DELETE /api/studentrecords/{id}` - delete record
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
