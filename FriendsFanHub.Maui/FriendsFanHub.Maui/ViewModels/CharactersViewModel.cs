using System.Collections.ObjectModel;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;

namespace FriendsFanHub.Maui.ViewModels;

public class CharactersViewModel : ViewModelBase
{
    private readonly IFriendsRepository _repository;
    private bool _loaded;

    public CharactersViewModel(IFriendsRepository repository)
    {
        _repository = repository;
    }

    public ObservableCollection<Character> Characters { get; } = new();

    public async Task LoadAsync(bool force = false)
    {
        if (_loaded && !force)
        {
            return;
        }

        IsBusy = true;
        var items = await _repository.GetCharactersAsync();
        Characters.Clear();
        foreach (var item in items)
        {
            Characters.Add(item);
        }

        _loaded = true;
        IsBusy = false;
    }

    public Task RefreshAsync() => LoadAsync(true);
}
