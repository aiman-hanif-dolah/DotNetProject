using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;
using FriendsFanHub.Maui.ViewModels;
using Microsoft.Maui.ApplicationModel;

namespace FriendsFanHub.Maui.Pages;

public partial class CharactersPage : ContentPage
{
    private readonly CharactersViewModel _viewModel;
    private readonly IFriendsRepository _repository;

    public CharactersPage()
    {
        InitializeComponent();
        _viewModel = ServiceHelper.GetService<CharactersViewModel>() ?? throw new InvalidOperationException("CharactersViewModel is not registered.");
        _repository = ServiceHelper.GetService<IFriendsRepository>() ?? throw new InvalidOperationException("Repository not registered.");
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync(true);
    }

    private async void OnAddCharacter(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CharacterFormPage));
    }

    private async void OnEditCharacter(object sender, EventArgs e)
    {
        if (TryGetId(sender, out var id))
        {
            await Shell.Current.GoToAsync($"{nameof(CharacterFormPage)}?id={id}");
        }
    }

    private async void OnDeleteCharacter(object sender, EventArgs e)
    {
        if (TryGetId(sender, out var id))
        {
            var confirm = await DisplayAlert("Delete", "Delete this character?", "Delete", "Cancel");
            if (!confirm)
            {
                return;
            }

            try
            {
                await _repository.DeleteCharacterAsync(id);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            await _viewModel.RefreshAsync();
        }
    }

    private async void OnViewCharacter(object sender, EventArgs e)
    {
        var character = GetCharacter(sender);
        if (character is null)
        {
            return;
        }

        var details = $"{character.ActorName}\n{character.Occupation}\n\n{character.Description}";
        await DisplayAlert(character.Name, details, "Close");
    }

    private async void OnWatchCharacterClip(object sender, EventArgs e)
    {
        var character = GetCharacter(sender);
        if (character is null)
        {
            return;
        }

        var url = GetCharacterVideoUrl(character);
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

    private static Character? GetCharacter(object sender) =>
        sender switch
        {
            Button { CommandParameter: Character character } => character,
            BindableObject { BindingContext: Character bound } => bound,
            _ => null
        };

    private static string GetCharacterVideoUrl(Character character)
    {
        if (!string.IsNullOrWhiteSpace(character.VideoUrl))
        {
            return character.VideoUrl;
        }

        var encoded = Uri.EscapeDataString($"{character.Name} Friends scene");
        return $"https://www.youtube.com/embed?listType=search&list={encoded}";
    }
}
