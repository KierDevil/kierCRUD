using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using DepartmentFinancialRecords.API.Data;

var builder = WebApplication.CreateBuilder(args);
var configuredConnection = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=DepartmentFinancialRecords.db";
var configuredProvider = builder.Configuration["Database:Provider"] ?? Environment.GetEnvironmentVariable("DB_PROVIDER");
var mysqlHost = builder.Configuration["Database:Host"] ?? Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var mysqlPort = builder.Configuration["Database:Port"] ?? Environment.GetEnvironmentVariable("DB_PORT") ?? "3307";
var mysqlDatabase = builder.Configuration["Database:Name"] ?? Environment.GetEnvironmentVariable("DB_NAME") ?? "department_financial_records";
var mysqlUser = builder.Configuration["Database:Username"] ?? Environment.GetEnvironmentVariable("DB_USER") ?? Environment.GetEnvironmentVariable("DB_USERNAME") ?? "root";
var mysqlPassword = builder.Configuration["Database:Password"] ?? Environment.GetEnvironmentVariable("DB_PASSWORD");

var hasExplicitMySqlValues = !string.IsNullOrWhiteSpace(mysqlPassword) && mysqlPassword != "YourPasswordHere";
var hasMySqlConnectionString = configuredConnection.Contains("Server=", StringComparison.OrdinalIgnoreCase) || configuredConnection.Contains("Host=", StringComparison.OrdinalIgnoreCase);
var isMySql = (configuredProvider == "mysql" || hasExplicitMySqlValues || (hasMySqlConnectionString && !configuredConnection.Contains("YourPasswordHere", StringComparison.OrdinalIgnoreCase)));

var connection = isMySql
    ? $"Server={mysqlHost};Port={mysqlPort};Database={mysqlDatabase};User={mysqlUser};Password={mysqlPassword};"
    : configuredConnection;

if (isMySql) builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseMySql(connection, ServerVersion.AutoDetect(connection)));
else builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(connection));

var jwtKey = builder.Configuration["Jwt:Key"] ?? "ReplaceWithSecureKeyForLocalDevelopmentOnly";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o => { o.TokenValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), ValidateIssuer = false, ValidateAudience = false }; });
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var origins = builder.Configuration["AllowedCorsOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? ["http://localhost:5173"];
builder.Services.AddCors(o => o.AddPolicy("Frontend", p => p.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (isMySql)
    {
        var builderConnection = new MySqlConnectionStringBuilder(connection);
        var dbName = builderConnection.Database;
        if (!string.IsNullOrWhiteSpace(dbName))
        {
            builderConnection.Database = "mysql";
            using var adminConnection = new MySqlConnection(builderConnection.ConnectionString);
            adminConnection.Open();
            using var command = adminConnection.CreateCommand();
            command.CommandText = $"CREATE DATABASE IF NOT EXISTS `{dbName}`;";
            command.ExecuteNonQuery();
        }
    }

    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}
app.UseSwagger(); app.UseSwaggerUI(); app.UseCors("Frontend"); app.UseAuthentication(); app.UseAuthorization();
app.MapGet("/", () => Results.Ok(new
{
    status = "ok",
    app = "Department Financial Records API",
    health = "/api/health",
    swagger = "/swagger"
}));
app.MapControllers(); app.Run();
