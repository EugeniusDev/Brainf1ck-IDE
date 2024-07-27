using Brainf1ck_IDE.Presentation.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace Brainf1ck_IDE
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddTransient<ProjectPage>();
            builder.Services.AddTransient<ProjectPageViewModel>();

            builder.Services.AddTransient<VisualizerPage>();
            builder.Services.AddTransient<VisualizerPageViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
