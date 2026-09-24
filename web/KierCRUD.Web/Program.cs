using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using KierCRUD.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient<StudentRecordApiService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseProxy = false
    })
    .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(10));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // Don't require HTTPS in development
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
