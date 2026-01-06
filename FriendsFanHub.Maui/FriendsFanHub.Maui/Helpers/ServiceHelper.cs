using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace FriendsFanHub.Maui.Helpers;

public static class ServiceHelper
{
    public static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services.GetService<T>();
}
