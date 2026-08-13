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
        CREATE TABLE IF NOT EXISTS "StudentRecords" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_StudentRecords" PRIMARY KEY AUTOINCREMENT,
            "Name" TEXT NOT NULL,
            "Email" TEXT NOT NULL,
            "Amount" TEXT NOT NULL,
            "CreatedAt" TEXT NOT NULL
        );
        """);
}

app.UseCors("Frontend");
app.MapControllers();

app.Run();
