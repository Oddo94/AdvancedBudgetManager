using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManagerCore.view_model;
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

        public ConfirmEmailWindow([NotNull] EmailConfirmationViewModel emailConfirmationViewModel, [NotNull] ConfirmationCodeInputDialog confirmationCodeInputDialog) {
            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.confirmationCodeInputDialog = confirmationCodeInputDialog;

            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(500, 500));

            if (appWindow.Presenter is OverlappedPresenter appWindowPresenter) {
                appWindowPresenter.IsResizable = false;
            }

            this.InitializeComponent();
            //this.ResetPasswordContentFrame.Navigate(typeof(EmailInputPage));
        }

        public async void ConfirmEmailButton_Click(object sender, RoutedEventArgs e) {
            //ContentDialog confirmationCodeDialog = new ConfirmationCodeInputDialog() {
            //    XamlRoot = this.Content.XamlRoot
            //};

            this.confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;

            ContentDialogResult confirmationDialogResult = await confirmationCodeInputDialog.ShowAsync();

            if (confirmationDialogResult == ContentDialogResult.Primary) {
               //Add logic
            } else {
                Debug.WriteLine("User closed the email confirmation dialog.");
            }

            //string emailAddress = EmailAddressTextBox.Text;

            //ResetPasswordWindow resetPasswordWindow = new ResetPasswordWindow();
            //resetPasswordWindow.Activate();


            //MailAddress mailAddress = new MailAddress(emailAddress);

            //await confirmationCodeDialog.ShowAsync();
        }
    }
}
