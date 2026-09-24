# Department Financial Records API

ASP.NET Core 8 API for department financial records, collections, attendance, and reporting.

## Run

```powershell
cd C:\kierCRUD
.\start.cmd
```

API: http://localhost:5000  
Health: http://localhost:5000/api/health  
Swagger: http://localhost:5000/swagger

The default provider is SQLite with `Data Source=DepartmentFinancialRecords.db`. If `ConnectionStrings:DefaultConnection` is changed to a MySQL connection containing `Server=` or `Host=`, Pomelo MySQL is selected instead. Startup uses `EnsureCreated`, so MySQL-specific migration SQL is never run while SQLite is active.
