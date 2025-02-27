﻿using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Notify.Contracts.Services;
using Notify.Helpers;

using Windows.ApplicationModel;

namespace Notify.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private ElementTheme _elementTheme;
    private string _versionDescription;

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _elementTheme = themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        async void UpdateTheme(ElementTheme param)
        {
            if (ElementTheme == param)
            {
                return;
            }

            ElementTheme = param;
            await themeSelectorService.SetThemeAsync(param);
        }

        SwitchThemeCommand = new RelayCommand<ElementTheme>(UpdateTheme);
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
