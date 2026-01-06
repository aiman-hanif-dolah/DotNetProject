using System.Collections.ObjectModel;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;

namespace FriendsFanHub.Maui.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly IFriendsRepository _repository;
    private bool _loaded;
    private int _characterCount;
    private int _episodeCount;
    private int _quoteCount;
    private int _locationCount;
    private Quote? _randomQuote;

    public DashboardViewModel(IFriendsRepository repository)
    {
        _repository = repository;
    }

    public int CharacterCount
    {
        get => _characterCount;
        set => SetProperty(ref _characterCount, value);
    }

    public int EpisodeCount
    {
        get => _episodeCount;
        set => SetProperty(ref _episodeCount, value);
    }

    public int QuoteCount
    {
        get => _quoteCount;
        set => SetProperty(ref _quoteCount, value);
    }

    public int LocationCount
    {
        get => _locationCount;
        set => SetProperty(ref _locationCount, value);
    }

    public Quote? RandomQuote
    {
        get => _randomQuote;
        set => SetProperty(ref _randomQuote, value);
    }

    public ObservableCollection<Quote> RecentQuotes { get; } = new();

    public async Task LoadAsync(bool force = false)
    {
        if (_loaded && !force)
        {
            return;
        }

        IsBusy = true;
        var snapshot = await _repository.GetDashboardAsync();
        CharacterCount = snapshot.CharacterCount;
        EpisodeCount = snapshot.EpisodeCount;
        QuoteCount = snapshot.QuoteCount;
        LocationCount = snapshot.LocationCount;
        RandomQuote = snapshot.RandomQuote;

        RecentQuotes.Clear();
        foreach (var quote in snapshot.RecentQuotes)
        {
            RecentQuotes.Add(quote);
        }

        _loaded = true;
        IsBusy = false;
    }

    public Task RefreshAsync() => LoadAsync(true);
}
