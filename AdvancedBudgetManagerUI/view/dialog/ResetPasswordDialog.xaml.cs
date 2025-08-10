using AdvancedBudgetManagerCore.view_model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.dialog;

public sealed partial class ResetPasswordDialog : ContentDialog {
    private ResetPasswordViewModel resetPasswordViewModel;
    private EmailConfirmationViewModel emailConfirmationViewModel;

    public ResetPasswordDialog([NotNull] ResetPasswordViewModel resetPasswordViewModel, [NotNull] EmailConfirmationViewModel emailConfirmationViewModel) {
        this.resetPasswordViewModel = resetPasswordViewModel;
        this.emailConfirmationViewModel = emailConfirmationViewModel;

        this.InitializeComponent();
    }

    public async void ResetPasswordButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs e) {
        //Debug.WriteLine("Inside ResetPasswordButton_Click");

        try {
            string userEmail = emailConfirmationViewModel.RecipientEmailAddress;

            resetPasswordViewModel.ResetPassword(userEmail);

            //ContentDialog passwordResetSuccessDialog = new ContentDialog {
            //    Title = "Password reset",
            //    Content = "Your password was successfully reset!",
            //    CloseButtonText = "OK",
            //    XamlRoot = this.Content.XamlRoot
            //};
            //await passwordResetSuccessDialog.ShowAsync();


        } catch (SystemException ex) {
            Debug.WriteLine($"Password reset failed! Reason: {ex.Message}");

            //ContentDialog passwordResetErrorDialog = new ContentDialog {
            //    Title = "Reset password",
            //    Content = ex.Message,
            //    CloseButtonText = "OK",
            //    XamlRoot = this.Content.XamlRoot
            //};
            //await passwordResetErrorDialog.ShowAsync();
        }
    }

    private void ResetPasswordDialog_Loaded(object sender, RoutedEventArgs e) {      
    }

}
