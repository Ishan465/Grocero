using Grocero.ViewModels;
using Grocero.Views;
using Microsoft.Extensions.Logging;

namespace Grocero
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

            builder.Services.AddSingleton<MainPage>(); 
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<WelcomePage>();
            builder.Services.AddSingleton<WelcomeViewModel>();

            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<RegisterViewModel>();
#endif

            return builder.Build();
        }
    }
}
