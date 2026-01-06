using FriendsFanHub.Maui.Pages;

namespace FriendsFanHub.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("dashboard", typeof(DashboardPage));
        Routing.RegisterRoute("characters", typeof(CharactersPage));
        Routing.RegisterRoute("episodes", typeof(EpisodesPage));
        Routing.RegisterRoute("quotes", typeof(QuotesPage));
        Routing.RegisterRoute("locations", typeof(LocationsPage));
        Routing.RegisterRoute(nameof(CharacterFormPage), typeof(CharacterFormPage));
        Routing.RegisterRoute(nameof(EpisodeFormPage), typeof(EpisodeFormPage));
        Routing.RegisterRoute(nameof(LocationFormPage), typeof(LocationFormPage));
        Routing.RegisterRoute(nameof(QuoteFormPage), typeof(QuoteFormPage));
    }
}
