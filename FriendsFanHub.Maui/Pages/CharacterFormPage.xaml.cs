using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;

namespace FriendsFanHub.Maui.Pages;

public partial class CharacterFormPage : ContentPage, IQueryAttributable
{
    private readonly IFriendsRepository _repository;
    private int? _characterId;
    private Character _model = new();

    public CharacterFormPage()
    {
        InitializeComponent();
        _repository = ServiceHelper.GetService<IFriendsRepository>() ?? throw new InvalidOperationException("Repository not registered.");
        BindingContext = _model;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj) && int.TryParse(idObj?.ToString(), out var id))
        {
            _characterId = id;
            var existing = await _repository.GetCharacterAsync(id);
            if (existing != null)
            {
                _model = existing;
                BindingContext = _model;
            }
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_model.Name) || string.IsNullOrWhiteSpace(_model.ActorName))
        {
            await DisplayAlert("Validation", "Name and Actor are required.", "OK");
            return;
        }

        if (_characterId.HasValue)
        {
            await _repository.UpdateCharacterAsync(_model);
        }
        else
        {
            await _repository.AddCharacterAsync(_model);
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
