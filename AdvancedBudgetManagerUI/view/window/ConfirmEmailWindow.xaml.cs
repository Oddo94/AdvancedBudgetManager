using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManagerCore.view_model;
using Autofac.Features.AttributeFilters;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.window {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfirmEmailWindow : Window {
        private EmailConfirmationViewModel emailConfirmationViewModel;
        private ConfirmationCodeInputDialog confirmationCodeInputDialog;
        private ResetPasswordDialog resetPasswordDialog;

        public ConfirmEmailWindow([NotNull] EmailConfirmationViewModel emailConfirmationViewModel, [NotNull] ConfirmationCodeInputDialog confirmationCodeInputDialog, [NotNull] ResetPasswordDialog resetPasswordDialog) {
            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.confirmationCodeInputDialog = confirmationCodeInputDialog;
            this.resetPasswordDialog = resetPasswordDialog;

            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(500, 500));

            if (appWindow.Presenter is OverlappedPresenter appWindowPresenter) {
                appWindowPresenter.IsResizable = false;
            }

            this.InitializeComponent();;
        }

        public async void ConfirmEmailButton_Click(object sender, RoutedEventArgs e) {
            this.confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;

            ContentDialogResult confirmationDialogResult = await confirmationCodeInputDialog.ShowAsync();

            if (confirmationDialogResult == ContentDialogResult.Primary && confirmationCodeInputDialog.IsValidConfirmationCode) {
                await Task.Delay(50);
                resetPasswordDialog.XamlRoot = this.Content.XamlRoot;

                await resetPasswordDialog.ShowAsync();
            } else {
                Debug.WriteLine("User closed the email confirmation dialog.");
            }
        }
    }
}
