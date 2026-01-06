using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.Models;
using FriendsFanHub.Maui.Services;
using FriendsFanHub.Maui.ViewModels;

namespace FriendsFanHub.Maui.Pages;

public partial class QuotesPage : ContentPage
{
    private readonly QuotesViewModel _viewModel;
    private readonly IFriendsRepository _repository;

    public QuotesPage()
    {
        InitializeComponent();
        _viewModel = ServiceHelper.GetService<QuotesViewModel>() ?? throw new InvalidOperationException("QuotesViewModel is not registered.");
        _repository = ServiceHelper.GetService<IFriendsRepository>() ?? throw new InvalidOperationException("Repository not registered.");
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync(true);
    }

    private async void OnAddQuote(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(QuoteFormPage));
    }

    private async void OnEditQuote(object sender, EventArgs e)
    {
        if (TryGetId(sender, out var id))
        {
            await Shell.Current.GoToAsync($"{nameof(QuoteFormPage)}?id={id}");
        }
    }

    private async void OnDeleteQuote(object sender, EventArgs e)
    {
        if (TryGetId(sender, out var id))
        {
            var confirm = await DisplayAlert("Delete", "Delete this quote?", "Delete", "Cancel");
            if (!confirm)
            {
                return;
            }

            try
            {
                await _repository.DeleteQuoteAsync(id);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            await _viewModel.RefreshAsync();
        }
    }

    private async void OnViewQuote(object sender, EventArgs e)
    {
        var quote = GetQuote(sender);
        if (quote is null)
        {
            return;
        }

        var details = $"“{quote.Text}”\n\n{quote.Context}\n\nCharacter: {quote.Character?.Name}\nEpisode: {quote.Episode?.EpisodeCode} — {quote.Episode?.Title}";
        await DisplayAlert("Quote", details, "Close");
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

    private static Quote? GetQuote(object sender) =>
        sender switch
        {
            Button { CommandParameter: Quote quote } => quote,
            BindableObject { BindingContext: Quote quote } => quote,
            _ => null
        };
}
