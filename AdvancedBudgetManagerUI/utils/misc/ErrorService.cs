using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace AdvancedBudgetManager.utils.misc {
    public class ErrorService : IErrorService {
        private IWindowProvider windowProvider;

        public ErrorService(IWindowProvider windowProvider) {
            this.windowProvider = windowProvider;
        }

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
