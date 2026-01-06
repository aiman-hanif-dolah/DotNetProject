using System.IO;
using FriendsFanHub.Maui.Data;
using FriendsFanHub.Maui.Pages;
using FriendsFanHub.Maui.Services;
using FriendsFanHub.Maui.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

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

		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "friendsfanhub.db3");
		builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));
		builder.Services.AddScoped<IFriendsRepository, FriendsRepository>();

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

		var app = builder.Build();
		EnsureDatabase(app.Services);
		return app;
	}

        private static void EnsureDatabase(IServiceProvider services)
        {
                using var scope = services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
        }
}
