using System.Collections.ObjectModel;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;

namespace FriendsFanHub.Maui.ViewModels;

public class EpisodesViewModel : ViewModelBase
{
    private readonly IFriendsRepository _repository;
    private bool _loaded;

    public EpisodesViewModel(IFriendsRepository repository)
    {
        _repository = repository;
    }

    public ObservableCollection<Episode> Episodes { get; } = new();

    public async Task LoadAsync(bool force = false)
    {
        if (_loaded && !force)
        {
            return;
        }

        IsBusy = true;
        var items = await _repository.GetEpisodesAsync();
        Episodes.Clear();
        foreach (var item in items)
        {
            Episodes.Add(item);
        }

        _loaded = true;
        IsBusy = false;
    }

    public Task RefreshAsync() => LoadAsync(true);
}
