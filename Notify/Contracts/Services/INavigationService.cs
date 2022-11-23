using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Notify.Contracts.Services;

public interface INavigationService
{
    event NavigatedEventHandler Navigated;

    Frame? Frame
    {
        get; set;
    }

    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    void SetListDataItemForNextConnectedAnimation(object item);
}
