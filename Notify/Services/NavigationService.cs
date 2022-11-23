using System.Diagnostics.CodeAnalysis;

using CommunityToolkit.WinUI.UI.Animations;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Notify.Contracts.Services;
using Notify.Contracts.ViewModels;
using Notify.Helpers;

namespace Notify.Services;

// For more information on navigation between pages see
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/navigation.md
public class NavigationService : INavigationService
{
    private readonly IPageService _pageService;
    private object? _lastParameterUsed;
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;

    public Frame? Frame
    {
        get
        {
            if (_frame != null)
            {
                return _frame;
            }

            _frame = App.MainWindow.Content as Frame;
            RegisterFrameEvents();

            return _frame;
        }

        set
        {
            UnRegisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    public NavigationService(IPageService pageService)
    {
        _pageService = pageService;
    }

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    private void UnRegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = _pageService.GetPageType(pageKey);

        if (_frame == null || (_frame.Content?.GetType() == pageType &&
                               (parameter == null || parameter.Equals(_lastParameterUsed))))
        {
            return false;
        }

        _frame.Tag = clearNavigation;
        var vmBeforeNavigation = _frame.GetPageViewModel();
        var navigated = _frame.Navigate(pageType, parameter);
        if (!navigated)
        {
            return navigated;
        }

        _lastParameterUsed = parameter;
        if (vmBeforeNavigation is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
        }

        return navigated;

    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is not Frame frame)
        {
            return;
        }

        if (frame.GetPageViewModel() is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(e.Parameter);
        }

        Navigated?.Invoke(sender, e);
    }

    public void SetListDataItemForNextConnectedAnimation(object item) => Frame.SetListDataItemForNextConnectedAnimation(item);
}
