using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.view_model;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics.CodeAnalysis;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.window {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfirmEmailWindow : Window, IRecipient<RequestPasswordResetEmailConfirmationMessage> {
        private SharedPropertiesViewModelWrapper changePasswordViewModelWrapper;
        private EmailConfirmationViewModel emailConfirmationViewModel;
        private ConfirmationCodeInputDialog confirmationCodeInputDialog;
        private IWindowNavigationService windowNavigationService;
        private XamlRoot? baseWindowXamlRoot;

        public ConfirmEmailWindow(
            [NotNull] ConfirmationCodeInputDialog confirmationCodeInputDialog,
            [NotNull] SharedPropertiesViewModelWrapper changePasswordViewModelWrapper,
            [NotNull] IWindowNavigationService windowNavigationService) {

            this.confirmationCodeInputDialog = confirmationCodeInputDialog;
            this.changePasswordViewModelWrapper = changePasswordViewModelWrapper;
            this.windowNavigationService = windowNavigationService;

            emailConfirmationViewModel = this.changePasswordViewModelWrapper.PasswordResetEmailConfirmationVM;

            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(500, 500));

            if (appWindow.Presenter is OverlappedPresenter appWindowPresenter) {
                appWindowPresenter.IsResizable = false;
            }

            //Registers the window so that the MVVM Toolkit messaging system can work properly
            WeakReferenceMessenger.Default.Register(this);

            this.InitializeComponent();
        }

        public async void Receive(RequestPasswordResetEmailConfirmationMessage message) {
            confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;

            ContentDialogResult displayResult = await confirmationCodeInputDialog.ShowAsync();

            do {
                if (displayResult == ContentDialogResult.Primary) {
                    EmailConfirmationResponse emailConfirmationResponse = new EmailConfirmationResponse(confirmationCodeInputDialog.ConfirmationCode);

                    WeakReferenceMessenger.Default.Send(new EmailConfirmationSubmittedMessage(emailConfirmationResponse));

                    if (!emailConfirmationViewModel.IsConfirmationCodeMatch) {
                        confirmationCodeInputDialog.ShowErrorTipOnLoad = true;

                        displayResult = await confirmationCodeInputDialog.ShowAsync();
                    } else {
                        emailConfirmationViewModel.IsConfirmationCodeMatch = true;
                        windowNavigationService.Show(WindowKey.RESET_PASSWORD_WINDOW);

                        this.Close();
                    }
                }
            } while (!emailConfirmationViewModel.IsConfirmationCodeMatch);

            message.Reply(true);
        }

        public XamlRoot BaseWindowXamlRoot {
            set { this.baseWindowXamlRoot = value; }
        }
    }
}
