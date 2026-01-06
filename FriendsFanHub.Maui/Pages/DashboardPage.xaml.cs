using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.ViewModels;

namespace FriendsFanHub.Maui.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _viewModel;

    public DashboardPage()
    {
        InitializeComponent();
        _viewModel = ServiceHelper.GetService<DashboardViewModel>() ?? throw new InvalidOperationException("DashboardViewModel is not registered.");
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync(true);
    }
}
