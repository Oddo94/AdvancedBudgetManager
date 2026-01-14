using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
#pragma warning disable 1591
    /// <summary>
    /// Represents the view model for the email confirmation dialog.
    /// </summary>
    public partial class EmailConfirmationViewModel : ObservableObject, IRecipient<EmailConfirmationSubmittedMessage> {
        [ObservableProperty]
        private string emailAddress;

        private EmailService emailService;
        private IErrorService errorService;

        private IConfirmationNotifier emailConfirmationNotifier;
        private bool isConfirmationCodeMatch;
        private bool hasSentEmail;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailConfirmationViewModel"/> with default values.
        /// </summary>
        public EmailConfirmationViewModel([NotNull] EmailService emailService,
            [NotNull] IConfirmationNotifier emailConfirmationNotifier,
            [NotNull] IErrorService errorService) {
            this.emailService = emailService;
            this.errorService = errorService;
            this.emailConfirmationNotifier = emailConfirmationNotifier;

            isConfirmationCodeMatch = false;
            hasSentEmail = false;

            WeakReferenceMessenger.Default.Register<EmailConfirmationSubmittedMessage>(this);
        }

        /// <summary>
        /// Sends the confirmation code to the user email address.
        /// </summary>
        /// <exception cref="SystemException"></exception>
        public void SendEmail(EmailPurpose emailPurpose) {
            if (true) {
                throw new SystemException("This is custom exception for test purposes");
            }
            GenericResponse emailSendingResult = emailService.SendEmail(EmailAddress, emailPurpose);
        }

        [RelayCommand]
        private void RequestUserConfirmationCode(EmailPurpose emailPurpose) {
            try {
                if (!hasSentEmail) {
                    //CHECK METHOD BEHAVIOR ON EMAIL SENDING ERROR!!! 
                    SendEmail(emailPurpose);
                    hasSentEmail = true;
                }

                emailConfirmationNotifier.Notify();

            } catch (SystemException ex) {
                ErrorInfo errorInfo = new ErrorInfo("Error", "Failed to send the confirmation code to the specified email address.", ErrorSeverity.ERROR);
                errorService.Notify(errorInfo);
            }
        }

        ///<inheritdoc/>
        public void Receive(EmailConfirmationSubmittedMessage message) {
            String inputConfirmationCode = message.ConfirmationCode;

            string generatedConfirmationCode = emailService.GeneratedConfirmationCode;
            if (!ConfirmationCodesMatch(inputConfirmationCode, generatedConfirmationCode)) {
                isConfirmationCodeMatch = false;
            } else {
                isConfirmationCodeMatch = true;
            }
        }

        /// <summary>
        /// Checks if the input confirmation code matches the one that was generated.
        /// </summary>
        /// <param name="inputConfirmationCode">The confirmation code provided by the user.</param>
        /// <param name="generatedConfirmationCode">The actual confirmation code that was generated.</param>
        /// <returns>A <see cref="bool"/> value indicating if the two confirmation codes match.</returns>
        public bool ConfirmationCodesMatch([NotNull] string inputConfirmationCode, [NotNull] string generatedConfirmationCode) {
            return inputConfirmationCode.Equals(generatedConfirmationCode);
        }

        public bool IsConfirmationCodeMatch {
            get { return this.isConfirmationCodeMatch; }
            set { this.isConfirmationCodeMatch = value; }
        }
    }
}