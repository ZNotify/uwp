using System;

using Notify.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Notify.Views
{
    public sealed partial class MessagesDetailControl : UserControl
    {
        public SampleOrder ListMenuItem
        {
            get { return GetValue(ListMenuItemProperty) as SampleOrder; }
            set { SetValue(ListMenuItemProperty, value); }
        }

        public static readonly DependencyProperty ListMenuItemProperty = DependencyProperty.Register("ListMenuItem", typeof(SampleOrder), typeof(MessagesDetailControl), new PropertyMetadata(null, OnListMenuItemPropertyChanged));

        public MessagesDetailControl()
        {
            InitializeComponent();
        }

        private static void OnListMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MessagesDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
