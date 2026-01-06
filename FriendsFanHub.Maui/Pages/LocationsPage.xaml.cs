using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.Services;
using FriendsFanHub.Maui.ViewModels;
using Microsoft.Maui.ApplicationModel;
using LocationModel = FriendsFanHub.Maui.Models.Location;

namespace FriendsFanHub.Maui.Pages;

public partial class LocationsPage : ContentPage
{
    private readonly LocationsViewModel _viewModel;
    private readonly IFriendsRepository _repository;

    public LocationsPage()
    {
        InitializeComponent();
        _viewModel = ServiceHelper.GetService<LocationsViewModel>() ?? throw new InvalidOperationException("LocationsViewModel is not registered.");
        _repository = ServiceHelper.GetService<IFriendsRepository>() ?? throw new InvalidOperationException("Repository not registered.");
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync(true);
    }

    private async void OnAddLocation(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LocationFormPage));
    }

    private async void OnEditLocation(object sender, EventArgs e)
    {
        if (TryGetId(sender, out var id))
        {
            await Shell.Current.GoToAsync($"{nameof(LocationFormPage)}?id={id}");
        }
    }

    private async void OnDeleteLocation(object sender, EventArgs e)
    {
        if (TryGetId(sender, out var id))
        {
            var confirm = await DisplayAlert("Delete", "Delete this location?", "Delete", "Cancel");
            if (!confirm)
            {
                return;
            }

            await _repository.DeleteLocationAsync(id);
            await _viewModel.RefreshAsync();
        }
    }

    private async void OnViewLocation(object sender, EventArgs e)
    {
        var location = GetLocation(sender);
        if (location is null)
        {
            return;
        }

        var summary = $"{location.Type}\n{location.Address}\n\n{location.Description}";
        await DisplayAlert(location.Name, summary, "Close");
    }

    private async void OnWatchLocationClip(object sender, EventArgs e)
    {
        var location = GetLocation(sender);
        if (location is null)
        {
            return;
        }

        var url = GetLocationVideoUrl(location);
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

    private static LocationModel? GetLocation(object sender) =>
        sender switch
        {
            Button { CommandParameter: LocationModel model } => model,
            BindableObject { BindingContext: LocationModel model } => model,
            _ => null
        };

    private static string GetLocationVideoUrl(LocationModel location)
    {
        if (!string.IsNullOrWhiteSpace(location.VideoUrl))
        {
            return location.VideoUrl;
        }

        var encoded = Uri.EscapeDataString($"{location.Name} Friends set");
        return $"https://www.youtube.com/embed?listType=search&list={encoded}";
    }
}
