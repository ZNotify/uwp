using System;

using Notify.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Notify.Views
{
    public sealed partial class MessagesPage : Page
    {
        public MessagesViewModel ViewModel { get; } = new MessagesViewModel();

        public MessagesPage()
        {
            InitializeComponent();
            Loaded += MessagesPage_Loaded;
        }

        private async void MessagesPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(ListDetailsViewControl.ViewState);
        }
    }
}
