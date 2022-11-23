using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Notify.Contracts.Services;
using Notify.Contracts.ViewModels;
using Notify.Core.Contracts.Services;
using Notify.Core.Models;

namespace Notify.ViewModels;

public class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {

    }

    public void OnNavigatedFrom()
    {
    }
}
