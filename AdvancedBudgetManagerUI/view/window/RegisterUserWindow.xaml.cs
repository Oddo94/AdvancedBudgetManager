using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.view_model;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.window {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterUserWindow : Window, IRecipient<RequestEmailConfirmationMessage> {
        private RegisterUserViewModel registerUserViewModel;
        private EmailConfirmationViewModel emailConfirmationViewModel;
        private ConfirmationCodeInputDialog confirmationCodeInputDialog;
        public RegisterUserWindow([NotNull] EmailConfirmationViewModel emailConfirmationViewModel, 
            [NotNull] ConfirmationCodeInputDialog confirmationCodeInputDialog,
            [NotNull] RegisterUserViewModel registerUserViewModel) {

            this.registerUserViewModel = registerUserViewModel;
            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.confirmationCodeInputDialog = confirmationCodeInputDialog;
            
            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(600, 600));

            OverlappedPresenter appWindowPresenter = (OverlappedPresenter)appWindow.Presenter;
            appWindowPresenter.IsResizable = false;

            InitializeComponent();
        }

        public async void Receive(RequestEmailConfirmationMessage message) {
            confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;

            ContentDialogResult displayResult = await confirmationCodeInputDialog.ShowAsync();

            do {
                if (displayResult == ContentDialogResult.Primary) {
                    EmailConfirmationResponse emailConfirmationResponse = new EmailConfirmationResponse(confirmationCodeInputDialog.ConfirmationCode);

                    WeakReferenceMessenger.Default.Send(new EmailConfirmationSubmittedMessage(emailConfirmationResponse)); ;
                    
                    //Add try-catch logic for the situation when the user already exists or the email address is associated to an existing account
                    if (!emailConfirmationViewModel.IsConfirmationCodeMatch) {
                        confirmationCodeInputDialog.ShowErrorTipOnLoad = true;

                        displayResult = await confirmationCodeInputDialog.ShowAsync();
                    } else {
                        emailConfirmationViewModel.IsConfirmationCodeMatch = true;
                        registerUserViewModel.RegisterUser();
                    }

                }

            } while (!emailConfirmationViewModel.IsConfirmationCodeMatch);

            message.Reply(true);
        }
    }
}
