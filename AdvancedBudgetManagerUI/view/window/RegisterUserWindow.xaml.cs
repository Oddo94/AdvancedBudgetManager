using AdvancedBudgetManager.utils;
using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using AdvancedBudgetManagerCore.utils.security;
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
    public sealed partial class RegisterUserWindow : Window, IRecipient<RequestUserRegistrationEmailConfirmationMessage> {
        private RegisterUserViewModel registerUserViewModel;
        private ConfirmationCodeInputDialog confirmationCodeInputDialog;
        private SharedPropertiesViewModelWrapper sharedPropertiesViewModelWrapper;
        private EmailConfirmationViewModel emailConfirmationViewModel;
        private InputDataValidator inputValidator;

        public RegisterUserWindow(
            [NotNull] ConfirmationCodeInputDialog confirmationCodeInputDialog,
            [NotNull] RegisterUserViewModel registerUserViewModel,
            [NotNull] SharedPropertiesViewModelWrapper sharedPropertiesViewModelWrapper) {

            this.registerUserViewModel = registerUserViewModel;
            this.confirmationCodeInputDialog = confirmationCodeInputDialog;
            this.sharedPropertiesViewModelWrapper = sharedPropertiesViewModelWrapper;
            this.emailConfirmationViewModel = sharedPropertiesViewModelWrapper.UserRegistrationEmailConfirmationVM;
            this.inputValidator = new InputDataValidator();

            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(600, 600));

            OverlappedPresenter appWindowPresenter = (OverlappedPresenter)appWindow.Presenter;
            appWindowPresenter.IsResizable = false;

            //Registers the window so that the MVVM Toolkit messaging system can work properly
            WeakReferenceMessenger.Default.Register(this);

            InitializeComponent();
        }

        public async void RegisterUserButton_Click(object sender, RoutedEventArgs e) {
            ContentDialog registerUserErrorDialog = new ContentDialog {
                Title = "Register user",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            if (inputValidator.IsEmpty(UserNameTextBox.Text) ||
                inputValidator.IsEmpty(PasswordBox.Password) ||
                inputValidator.IsEmpty(EmailAddressTextBox.Text)) {

                registerUserErrorDialog.Content = "Please fill in all the required fields!";
                await registerUserErrorDialog.ShowAsync();

                return;
            }

            if (!inputValidator.HasRequiredLength(PasswordBox.Password, SecurityConstants.MINIMUM_PASSWORD_LENGTH, ComparisonMode.LENIENT)) {
                registerUserErrorDialog.Content = $"Your password should be at least {SecurityConstants.MINIMUM_PASSWORD_LENGTH} characters long! Please try again.";
                await registerUserErrorDialog.ShowAsync();

                return;
            }

            if (!inputValidator.IsValidPassword(PasswordBox.Password)) {
                registerUserErrorDialog.Content = "Invalid password! Your password must contain:\n1.Lowercase and uppercase letters (a-zA-z) \n2.Digits (0-9) \n3.Special characters (@#$%<>?)";
                await registerUserErrorDialog.ShowAsync();

                return;
            }

            try {
                //Check that the username and email address are not already used
                registerUserViewModel.ValidateUserDataCommand.Execute(null);

                //Send the confirmation code to the specified email address for confirming the identity of the new user
                //confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;
                emailConfirmationViewModel.RequestUserConfirmationCodeCommand.Execute(EmailPurpose.REGISTER_USER_EMAIL);
            } catch (AdvancedBudgetManagerException ex) {
                registerUserErrorDialog.Content = ex.Message;
                await registerUserErrorDialog.ShowAsync();
            }
        }

        public async void Receive(RequestUserRegistrationEmailConfirmationMessage message) {
            confirmationCodeInputDialog.XamlRoot = this.Content.XamlRoot;

            ContentDialogResult displayResult = await confirmationCodeInputDialog.ShowAsync();

            string registerUserMessage = "Failed to create the new user!";
            do {
                if (displayResult == ContentDialogResult.Primary) {
                    EmailConfirmationResponse emailConfirmationResponse = new EmailConfirmationResponse(confirmationCodeInputDialog.ConfirmationCode);

                    WeakReferenceMessenger.Default.Send(new EmailConfirmationSubmittedMessage(emailConfirmationResponse)); ;

                    //Add try-catch logic for the situation when the user already exists or the email address is associated to an existing account
                    try {
                        if (!emailConfirmationViewModel.IsConfirmationCodeMatch) {
                            confirmationCodeInputDialog.ShowErrorTipOnLoad = true;

                            displayResult = await confirmationCodeInputDialog.ShowAsync();
                        } else {
                            emailConfirmationViewModel.IsConfirmationCodeMatch = true;
                            registerUserViewModel.RegisterUser();
                            registerUserMessage = "The new user was successfully created!";
                        }
                    } catch (SystemException ex) {
                        registerUserMessage = ex.Message;
                    }
                }

            } while (!emailConfirmationViewModel.IsConfirmationCodeMatch);

            ContentDialog registerUserContentDialog = new ContentDialog {
                Title = "Register user",
                Content = registerUserMessage,
                PrimaryButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await registerUserContentDialog.ShowAsync();

            //Closes the window if the user creation was succesfull
            if (emailConfirmationViewModel.IsConfirmationCodeMatch) {
                this.Close();
            }


            //message.Reply(true);
        }
    }
}
