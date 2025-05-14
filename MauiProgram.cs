using cloud_app_dev_exam_project.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace cloud_app_dev_exam_project;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
#if DEBUG
        builder.Logging.AddDebug();

        builder.Services.AddSingleton<DatabaseService>();
        //builder.Services.AddSingleton<BankService>();

#endif
        return builder.Build();
    }
}