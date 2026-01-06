using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;

namespace FriendsFanHub.Maui.Pages;

public partial class QuoteFormPage : ContentPage, IQueryAttributable
{
    private readonly IFriendsRepository _repository;
    private int? _quoteId;
    private Quote _model = new();
    private List<Character> _characters = new();
    private List<Episode> _episodes = new();

    public QuoteFormPage()
    {
        InitializeComponent();
        _repository = ServiceHelper.GetService<IFriendsRepository>() ?? throw new InvalidOperationException("Repository not registered.");
        BindingContext = _model;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _characters = await _repository.GetCharactersAsync();
        _episodes = await _repository.GetEpisodesAsync();
        CharacterPicker.ItemsSource = _characters;
        EpisodePicker.ItemsSource = _episodes;

        if (query.TryGetValue("id", out var idObj) && int.TryParse(idObj?.ToString(), out var id))
        {
            _quoteId = id;
            var existing = await _repository.GetQuoteAsync(id);
            if (existing != null)
            {
                _model = existing;
                BindingContext = _model;

                CharacterPicker.SelectedItem = _characters.FirstOrDefault(c => c.Id == existing.CharacterId);
                EpisodePicker.SelectedItem = _episodes.FirstOrDefault(e => e.Id == existing.EpisodeId);
            }
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_model.Text))
        {
            await DisplayAlert("Validation", "Quote text is required.", "OK");
            return;
        }

        if (CharacterPicker.SelectedItem is not Character character || EpisodePicker.SelectedItem is not Episode episode)
        {
            await DisplayAlert("Validation", "Please select a character and an episode.", "OK");
            return;
        }

        _model.CharacterId = character.Id;
        _model.EpisodeId = episode.Id;

        if (_quoteId.HasValue)
        {
            await _repository.UpdateQuoteAsync(_model);
        }
        else
        {
            await _repository.AddQuoteAsync(_model);
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
