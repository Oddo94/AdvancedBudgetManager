using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.view_model;
using Autofac.Features.AttributeFilters;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MySqlX.XDevAPI.Common;
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
    public sealed partial class ConfirmEmailWindow : Window, IRecipient<RequestEmailConfirmationMessage> {
        private EmailConfirmationViewModel emailConfirmationViewModel;
        private ChangePasswordViewModelWrapper changePasswordViewModelWrapper;
        private ConfirmationCodeInputDialog confirmationCodeInputDialog;       
        private ResetPasswordDialog resetPasswordDialog;
        private XamlRoot baseWindowXamlRoot;

        public ConfirmEmailWindow(
            [NotNull] EmailConfirmationViewModel emailConfirmationViewModel,
            [NotNull] ConfirmationCodeInputDialog confirmationCodeInputDialog, 
            [NotNull] ResetPasswordDialog resetPasswordDialog, 
            [NotNull] ChangePasswordViewModelWrapper changePasswordViewModelWrapper) {

            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.confirmationCodeInputDialog = confirmationCodeInputDialog;
            this.resetPasswordDialog = resetPasswordDialog;
            this.changePasswordViewModelWrapper = changePasswordViewModelWrapper;

            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(500, 500));

            if (appWindow.Presenter is OverlappedPresenter appWindowPresenter) {
                appWindowPresenter.IsResizable = false;
            }

            WeakReferenceMessenger.Default.Register<RequestEmailConfirmationMessage>(this);
            this.InitializeComponent();
        }

        //OLD CODE VERSION
        //public async void ConfirmEmailButton_Click(object sender, RoutedEventArgs e) {
        //    this.confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;

        //    ContentDialogResult confirmationDialogResult = await confirmationCodeInputDialog.ShowAsync();

        //    //this.Close();

        //    if (confirmationDialogResult == ContentDialogResult.Primary && confirmationCodeInputDialog.IsValidConfirmationCode) {
        //        await Task.Delay(50);
        //        //resetPasswordDialog.XamlRoot = this.Content.XamlRoot;
        //        resetPasswordDialog.XamlRoot = baseWindowXamlRoot;

        //        await resetPasswordDialog.ShowAsync();
        //    } else {
        //        Debug.WriteLine("User closed the email confirmation dialog.");
        //    }
        //}
        //public async void ConfirmEmailButton_Click(object sender, RoutedEventArgs e) {
        //    WeakReferenceMessenger.Default.Register<EmailConfirmationSubmittedMessage>(this, async (r, m) => {
        //        var result = await ShowEmailConfirmationDialog(m.PromptTitle);
        //        if (result != null) {
        //            m.Reply(result);
        //        }
        //    });
        //}

        //private async Task<EmailConfirmationResponse> ShowEmailConfirmationDialog(string title) {
        //    confirmationCodeInputDialog.Title = title;
        //    confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;

        //    var result = await confirmationCodeInputDialog.ShowAsync();

        //    return result == ContentDialogResult.Primary ? this.confirmationCodeInputDialog.
        //}

        public async void Receive(RequestEmailConfirmationMessage message) {
            confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;

            ContentDialogResult displayResult = await confirmationCodeInputDialog.ShowAsync();

            if (displayResult == ContentDialogResult.Primary) {
                EmailConfirmationResponse emailConfirmationResponse = new EmailConfirmationResponse(confirmationCodeInputDialog.ConfirmationCode);

                WeakReferenceMessenger.Default.Send(new EmailConfirmationSubmittedMessage(emailConfirmationResponse));
            }

            message.Reply(true);
        }

        public XamlRoot BaseWindowXamlRoot {
            set { this.baseWindowXamlRoot = value; }
        }
    }
}
