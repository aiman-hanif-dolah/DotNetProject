using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;

namespace FriendsFanHub.Maui.Pages;

public partial class EpisodeFormPage : ContentPage, IQueryAttributable
{
    private readonly IFriendsRepository _repository;
    private int? _episodeId;
    private Episode _model = new();

    public EpisodeFormPage()
    {
        InitializeComponent();
        _repository = ServiceHelper.GetService<IFriendsRepository>() ?? throw new InvalidOperationException("Repository not registered.");
        AirDatePicker.Date = DateTime.Today;
        BindingContext = _model;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj) && int.TryParse(idObj?.ToString(), out var id))
        {
            _episodeId = id;
            var existing = await _repository.GetEpisodeAsync(id);
            if (existing != null)
            {
                _model = existing;
                BindingContext = _model;
                AirDatePicker.Date = existing.AirDate ?? DateTime.Today;
            }
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_model.Title))
        {
            await DisplayAlert("Validation", "Title is required.", "OK");
            return;
        }

        _model.AirDate = AirDatePicker.Date;

        if (!int.TryParse(SeasonEntry.Text, out var season) || season <= 0)
        {
            await DisplayAlert("Validation", "Season must be a positive number.", "OK");
            return;
        }

        if (!int.TryParse(EpisodeEntry.Text, out var episodeNumber) || episodeNumber <= 0)
        {
            await DisplayAlert("Validation", "Episode number must be a positive number.", "OK");
            return;
        }

        _model.Season = season;
        _model.EpisodeNumber = episodeNumber;

        if (_episodeId.HasValue)
        {
            await _repository.UpdateEpisodeAsync(_model);
        }
        else
        {
            await _repository.AddEpisodeAsync(_model);
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
