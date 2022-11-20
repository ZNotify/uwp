using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Notify.Activation;
using Notify.Core.Helpers;

using Windows.ApplicationModel.Activation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Notify.Services
{
    // For more information on understanding and extending activation flow see
    // https://github.com/microsoft/TemplateStudio/blob/main/docs/UWP/activation.md
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Type _defaultNavItem;
        private readonly Lazy<UIElement> _shell;

        private object _lastActivationArgs;

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            var isInteractive = IsInteractive(activationArgs);
            if (isInteractive)
            {
                // Initialize services that you need before app activation
                // take into account that the splash screen is shown while this code runs.
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (App.Window.Content == null)
                {
                    // Create a Shell or Frame to act as the navigation context
                    App.Window.Content = _shell?.Value ?? new Frame();
                }
            }

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            await HandleActivationAsync(activationArgs);
            _lastActivationArgs = activationArgs;

            if (isInteractive)
            {
                // Ensure the current window is active
                App.Window.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }

        private async Task InitializeAsync()
        {
            await Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasksAsync().ConfigureAwait(false);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }
            }
        }

        private async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();

            // TODO: Configure and enable Azure Notification Hub integration.
            //  1. Go to the HubNotificationsService class, in the InitializeAsync() method, provide the Hub Name and DefaultListenSharedAccessSignature.
            //  2. Uncomment the following line (an exception will be thrown if it is executed and the above information is not provided).
            // await Singleton<HubNotificationsService>.Instance.InitializeAsync().ConfigureAwait(false);
            await Task.CompletedTask;
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<NotificationsService>.Instance;
            yield return Singleton<BackgroundTaskService>.Instance;
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }
    }
}
