using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using Notify.Contracts.Services;
using Notify.ViewModels;
using Notify.Views;

namespace Notify.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Register<MainViewModel, MainPage>();
        Register<ListDetailsViewModel, ListDetailsPage>();
        Register<SettingsViewModel, SettingsPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Register?");
            }
        }

        return pageType;
    }

    private void Register<TObservableObject, TPage>()
        where TObservableObject : ObservableObject
        where TPage : Page
    {
        lock (_pages)
        {
            var key = typeof(TObservableObject).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(TPage);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
