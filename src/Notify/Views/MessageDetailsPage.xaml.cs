using System;

using Notify.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Notify.Views
{
    public sealed partial class MessageDetailsPage : Page
    {
        public MessageDetailsViewModel ViewModel { get; } = new MessageDetailsViewModel();

        public MessageDetailsPage()
        {
            InitializeComponent();
            Loaded += MessageDetailsPage_Loaded;
        }

        private async void MessageDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(ListDetailsViewControl.ViewState);
        }
    }
}
