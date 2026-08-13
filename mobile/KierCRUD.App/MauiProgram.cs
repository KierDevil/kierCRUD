using KierCRUD.App.Services;

namespace KierCRUD.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<StudentRecordApiService>();

        return builder.Build();
    }
}
