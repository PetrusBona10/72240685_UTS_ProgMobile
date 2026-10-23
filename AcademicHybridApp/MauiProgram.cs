using AcademicHybridApp.Components.Shared;
using AcademicHybridApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AcademicHybridApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddScoped<ToastService>();

		builder.Services.AddHttpClient<IDosenService, DosenService>(client =>
		{
			client.BaseAddress = new Uri(ApiConfig.BaseAddress);
			client.Timeout = TimeSpan.FromSeconds(10);
		});

		builder.Services.AddHttpClient<IMahasiswaService, MahasiswaService>(client =>
		{
			client.BaseAddress = new Uri(ApiConfig.BaseAddress);
			client.Timeout = TimeSpan.FromSeconds(10);
		});

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
