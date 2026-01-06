using System.Collections.ObjectModel;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;
using LocationModel = FriendsFanHub.Maui.Models.Location;

namespace FriendsFanHub.Maui.ViewModels;

public class LocationsViewModel : ViewModelBase
{
    private readonly IFriendsRepository _repository;
    private bool _loaded;

    public LocationsViewModel(IFriendsRepository repository)
    {
        _repository = repository;
    }

    public ObservableCollection<LocationModel> Locations { get; } = new();

    public async Task LoadAsync(bool force = false)
    {
        if (_loaded && !force)
        {
            return;
        }

        IsBusy = true;
        var items = await _repository.GetLocationsAsync();
        Locations.Clear();
        foreach (var item in items)
        {
            Locations.Add(item);
        }

        _loaded = true;
        IsBusy = false;
    }

    public Task RefreshAsync() => LoadAsync(true);
}
