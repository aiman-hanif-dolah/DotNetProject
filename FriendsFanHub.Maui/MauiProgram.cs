using FriendsFanHub.Maui.Pages;
using FriendsFanHub.Maui.Services;
using FriendsFanHub.Maui.ViewModels;
using Microsoft.Extensions.Logging;

namespace FriendsFanHub.Maui;

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

                builder.Services.AddHttpClient();
                builder.Services.AddSingleton(sp =>
                {
                        var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
                        return new FirestoreRestClient(
                                httpClient,
                                Helpers.AppConfig.FirestoreProjectId,
                                Helpers.AppConfig.FirestoreApiKey);
                });
                builder.Services.AddScoped<IFriendsRepository, FirestoreFriendsRepository>();

		builder.Services.AddTransient<DashboardViewModel>();
		builder.Services.AddTransient<CharactersViewModel>();
		builder.Services.AddTransient<EpisodesViewModel>();
		builder.Services.AddTransient<LocationsViewModel>();
		builder.Services.AddTransient<QuotesViewModel>();

		builder.Services.AddTransient<DashboardPage>();
		builder.Services.AddTransient<CharactersPage>();
		builder.Services.AddTransient<EpisodesPage>();
		builder.Services.AddTransient<LocationsPage>();
		builder.Services.AddTransient<QuotesPage>();
		builder.Services.AddTransient<CharacterFormPage>();
		builder.Services.AddTransient<EpisodeFormPage>();
		builder.Services.AddTransient<LocationFormPage>();
		builder.Services.AddTransient<QuoteFormPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

                return builder.Build();
        }
}
