using BZ.Pages;
using BZ.ViewModels;
using CommunityToolkit.Maui;
using DataAccess.Services;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BZ
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

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<AuthHeaderHandler>();
            builder.Services.AddSingleton<IProcedureService, ProcedureService>();
            builder.Services.AddSingleton<IBeautyTechService, BeautyTechService>();

            builder.Services.AddHttpClient("AppHttpClient", client =>
            {
                client.BaseAddress = new Uri(Constants.BaseApiUrl);
            });

            builder.Services.AddHttpClient("HttpClient", client =>
            {
                client.BaseAddress = new Uri(Constants.BaseApiUrl);
            }).AddHttpMessageHandler<AuthHeaderHandler>();
            

            builder.Services.AddScoped<LoginPageViewModel>();
            builder.Services.AddScoped<HomePageViewModel>();
            builder.Services.AddScoped<ProcedurePageViewModel>();
            builder.Services.AddScoped<BeautyTechViewModel>();
            builder.Services.AddScoped<BeautyTechFormViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProcedurePage>();
            builder.Services.AddTransient<BeautyTechsPage>();
            builder.Services.AddTransient<BeautyTechFormPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
