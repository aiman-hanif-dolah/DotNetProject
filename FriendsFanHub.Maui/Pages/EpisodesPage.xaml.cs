using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;
using FriendsFanHub.Maui.ViewModels;
using Microsoft.Maui.ApplicationModel;

namespace FriendsFanHub.Maui.Pages;

public partial class EpisodesPage : ContentPage
{
    private readonly EpisodesViewModel _viewModel;
    private readonly IFriendsRepository _repository;

    public EpisodesPage()
    {
        InitializeComponent();
        _viewModel = ServiceHelper.GetService<EpisodesViewModel>() ?? throw new InvalidOperationException("EpisodesViewModel is not registered.");
        _repository = ServiceHelper.GetService<IFriendsRepository>() ?? throw new InvalidOperationException("Repository not registered.");
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync(true);
    }

    private async void OnAddEpisode(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EpisodeFormPage));
    }

    private async void OnEditEpisode(object sender, EventArgs e)
    {
        if (TryGetId(sender, out var id))
        {
            await Shell.Current.GoToAsync($"{nameof(EpisodeFormPage)}?id={id}");
        }
    }

    private async void OnDeleteEpisode(object sender, EventArgs e)
    {
        if (TryGetId(sender, out var id))
        {
            var confirm = await DisplayAlert("Delete", "Delete this episode?", "Delete", "Cancel");
            if (!confirm)
            {
                return;
            }

            try
            {
                await _repository.DeleteEpisodeAsync(id);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            await _viewModel.RefreshAsync();
        }
    }

    private async void OnViewEpisode(object sender, EventArgs e)
    {
        var episode = GetEpisode(sender);
        if (episode is null)
        {
            return;
        }

        var airDate = episode.AirDate?.ToString("MMM dd, yyyy") ?? "N/A";
        var details = $"{episode.EpisodeCode} • Air date: {airDate}\n\n{episode.Description}";
        await DisplayAlert(episode.Title, details, "Close");
    }

    private async void OnWatchEpisodeClip(object sender, EventArgs e)
    {
        var episode = GetEpisode(sender);
        if (episode is null)
        {
            return;
        }

        var url = GetEpisodeVideoUrl(episode);
        try
        {
            await Launcher.OpenAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to open video link: {ex.Message}", "OK");
        }
    }

    private static bool TryGetId(object sender, out int id)
    {
        id = 0;
        switch (sender)
        {
            case SwipeItem swipe when swipe.CommandParameter is int swipeId:
                id = swipeId;
                return true;
            case Button { CommandParameter: int buttonId }:
                id = buttonId;
                return true;
        }

        return false;
    }

    private static Episode? GetEpisode(object sender) =>
        sender switch
        {
            Button { CommandParameter: Episode ep } => ep,
            BindableObject { BindingContext: Episode ep } => ep,
            _ => null
        };

    private static string GetEpisodeVideoUrl(Episode episode)
    {
        if (!string.IsNullOrWhiteSpace(episode.VideoUrl))
        {
            return episode.VideoUrl;
        }

        var encoded = Uri.EscapeDataString($"{episode.EpisodeCode} Friends");
        return $"https://www.youtube.com/embed?listType=search&list={encoded}";
    }
}
