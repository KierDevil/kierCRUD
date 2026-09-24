# 🚀 Quick Start Guide - KierCRUD Web App

## The Fastest Way to Get Running

### 1️⃣ One Command to Start Everything
```powershell
.\STARTWEBSES.cmd
```

This will:
- Open a terminal for the Backend API
- Open a terminal for the Web App
- Display startup information

### 2️⃣ Wait 5-10 Seconds
The application needs time to:
- Compile the .NET code
- Start the API
- Start the web app

### 3️⃣ Open Your Browser
Navigate to:
```
http://localhost:5173
```

## That's It! ✅

You should see the KierCRUD dashboard with:
- 👥 Students management section
- 📝 Enrollments management section  
- 📚 Catalog management section

## Common Tasks

### Add a New Student
1. Click **"Manage Students"**
2. Click **"+ Add Student"**
3. Fill in Student ID, Name, and Status
4. Click **"Create"**

### Add a Course Enrollment
1. Click **"Manage Enrollments"**
2. Click **"+ Add Enrollment"**
3. Select Student, Course, Semester, School Year
4. Click **"Create"**

### Manage the Catalog
1. Click **"Manage Catalog"**
2. Click on a tab (School Years, Courses, or Semesters)
3. Add or delete items as needed

## If Something Goes Wrong

### "Cannot connect to API"
- Is the backend terminal showing errors?
- Check the terminal windows are still open
- Try closing both and running `.\STARTWEBSES.cmd` again

### "Page not loading"
- Check browser console: Press F12
- Look for red errors
- Refresh the page: Ctrl+R

### "Port 5173 already in use"
- Stop the current app (Ctrl+C in terminal)
- Run: `cd web\KierCRUD.Web && dotnet run --urls "http://localhost:5174"`
- Open in browser: `http://localhost:5174`

## Stop the Application

Press `Ctrl+C` in each terminal window to stop:
1. Backend API
2. Web App

Or close the terminal windows directly.

---

**Enjoy your new web-based KierCRUD! 🎉**
