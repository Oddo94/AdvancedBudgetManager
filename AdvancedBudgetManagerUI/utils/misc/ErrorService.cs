using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace AdvancedBudgetManager.utils.misc {
    /// <summary>
    /// Service class used for displaying error messages to the currebtly active window.
    /// </summary>
    public class ErrorService : IErrorService {
        /// <summary>
        /// The window provider instance.
        /// </summary>
        private IWindowProvider windowProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorService"/> class based on the specified window provider.
        /// </summary>
        /// <param name="windowProvider">The <see cref="IWindowProvider"/> instance used for retrieving information about the currently active window.</param>
        public ErrorService(IWindowProvider windowProvider) {
            this.windowProvider = windowProvider;
        }

        /// <inheritdoc/>
        public async void Notify(ErrorInfo errorInfo) {
            XamlRoot activeXamlRoot = windowProvider.GetActiveXamlRoot();

            ContentDialog errorDialog = new ContentDialog() {
                Title = errorInfo.Title,
                Content = errorInfo.Message,
                CloseButtonText = "OK",
                XamlRoot = activeXamlRoot
            };

            await errorDialog.ShowAsync();
        }
    }
}
