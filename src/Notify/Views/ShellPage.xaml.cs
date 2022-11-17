using System;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;
using Notify.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.UI.WindowManagement;

namespace Notify.Views
{
    // TODO: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();

            DataContext = ViewModel;
            ViewModel.Initialize(ShellFrame, NavigationView, KeyboardAccelerators);
        }
    }
}
