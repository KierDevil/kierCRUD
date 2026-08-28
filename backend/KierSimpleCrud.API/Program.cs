using KierSimpleCrud.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? "Data Source=kiercrud.db";

var allowedCorsOrigins = (builder.Configuration["AllowedCorsOrigins"]
    ?? Environment.GetEnvironmentVariable("ALLOWED_CORS_ORIGINS")
    ?? "http://localhost:5173")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        if (allowedCorsOrigins.Any(origin => origin == "*"))
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedCorsOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    dbContext.Database.ExecuteSqlRaw("""
        PRAGMA foreign_keys = ON;

        CREATE TABLE IF NOT EXISTS "Students" (
            "Studid" TEXT NOT NULL CONSTRAINT "PK_Students" PRIMARY KEY,
            "StudentName" TEXT NOT NULL,
            "Status" TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS "SchoolYears" (
            "Sycode" TEXT NOT NULL CONSTRAINT "PK_SchoolYears" PRIMARY KEY,
            "SchoolYear" TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS "Courses" (
            "Courscode" TEXT NOT NULL CONSTRAINT "PK_Courses" PRIMARY KEY,
            "CourseName" TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS "Semesters" (
            "Semcode" TEXT NOT NULL CONSTRAINT "PK_Semesters" PRIMARY KEY,
            "SemesterName" TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS "Enrollments" (
            "EnrollmentId" INTEGER NOT NULL CONSTRAINT "PK_Enrollments" PRIMARY KEY AUTOINCREMENT,
            "Studid" TEXT NOT NULL,
            "Sycode" TEXT NOT NULL,
            "Courscode" TEXT NOT NULL,
            "Semcode" TEXT NOT NULL,
            "Status" TEXT NOT NULL,
            "EnrollmentDate" TEXT NOT NULL,
            CONSTRAINT "FK_Enrollments_Students_Studid" FOREIGN KEY ("Studid") REFERENCES "Students" ("Studid") ON DELETE RESTRICT,
            CONSTRAINT "FK_Enrollments_SchoolYears_Sycode" FOREIGN KEY ("Sycode") REFERENCES "SchoolYears" ("Sycode") ON DELETE RESTRICT,
            CONSTRAINT "FK_Enrollments_Courses_Courscode" FOREIGN KEY ("Courscode") REFERENCES "Courses" ("Courscode") ON DELETE RESTRICT,
            CONSTRAINT "FK_Enrollments_Semesters_Semcode" FOREIGN KEY ("Semcode") REFERENCES "Semesters" ("Semcode") ON DELETE RESTRICT
        );

        CREATE UNIQUE INDEX IF NOT EXISTS "IX_Enrollments_Studid_Sycode_Semcode" ON "Enrollments" ("Studid", "Sycode", "Semcode");

        INSERT OR IGNORE INTO "SchoolYears" ("Sycode", "SchoolYear") VALUES
            ('SY2025', '2025-2026'),
            ('SY2026', '2026-2027');

        INSERT OR IGNORE INTO "Courses" ("Courscode", "CourseName") VALUES
            ('BSCS', 'Bachelor of Science in Computer Science'),
            ('BSIS', 'Bachelor of Science in Information Systems'),
            ('BLIS', 'Bachelor of Library and Information Science');

        INSERT OR IGNORE INTO "Semesters" ("Semcode", "SemesterName") VALUES
            ('1ST', '1st Semester'),
            ('2ND', '2nd Semester'),
            ('SUM', 'Summer');
        """);
}

app.UseCors("Frontend");
app.MapControllers();

app.Run();
