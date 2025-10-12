using AdvancedBudgetManagerCore.model;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.security;
using Autofac.Features.AttributeFilters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.view_model {
    #pragma warning disable 1591
    /// <summary>
    /// Represents the view model for the email confirmation dialog.
    /// </summary>
    public partial class EmailConfirmationViewModel : ObservableObject, IRecipient<EmailConfirmationSubmittedMessage> {
        [ObservableProperty]
        private string userEmail;

        //[ObservableProperty]
        //private string inputConfirmationCode;

        //private ICrudRepository emailConfirmationRepository;
        private EmailConfirmationSender emailConfirmationSender;
        private SecretReader secretReader;
        private string generatedConfirmationCode;
        private bool isConfirmationCodeMatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailConfirmationViewModel"/> with default values.
        /// </summary>
        public EmailConfirmationViewModel() {
            //this.emailConfirmationRepository = emailConfirmationRepository;
            this.emailConfirmationSender = new EmailConfirmationSender();
            this.secretReader = new SecretReader();
            //IsActive = true;
            isConfirmationCodeMatch = true;

            WeakReferenceMessenger.Default.Register<EmailConfirmationSubmittedMessage>(this);

        }

        /// <summary>
        /// Sends the confirmation code to the user email address.
        /// </summary>
        /// <exception cref="SystemException"></exception>
        public void SendEmailConfirmationCode() {
            EmailSenderCredentials emailSenderCredentials = secretReader.GetEmailSenderCredentials();

            string emailSubject = "BudgetManager-password reset";
            string emailBody = "A password reset was requested for the BudgetManager application account associated to this email address.\nPlease enter the following code to finish the password reset process: {0} \nIf you have not requested the password reset please ignore this email and delete it immediately.";
            int confirmationCodeSize = 32;
            generatedConfirmationCode = emailConfirmationSender.GenerateConfirmationCode(confirmationCodeSize);

            ConfirmationEmailDetails confirmationEmailDetails = new ConfirmationEmailDetails(UserEmail, emailSubject, emailBody, generatedConfirmationCode);

            try {
                emailConfirmationSender.SendConfirmationEmail(emailSenderCredentials, confirmationEmailDetails);
            } catch (Exception ex) {
                throw new SystemException("An error occurred while sending the confirmation code by email");
            }
        }

        [RelayCommand]
        private void RequestUserConfirmationCode() {
            //EmailConfirmationSubmittedMessage message = new EmailConfirmationMessage();


            if (isConfirmationCodeMatch) {
                //Sends the email confirmation code to the user's email address
                SendEmailConfirmationCode();
            }

            WeakReferenceMessenger.Default.Send(new RequestEmailConfirmationMessage());

            //if (message.HasReceivedResponse) {
            //    Task<EmailConfirmationResponse> response = message.Response;

            //    this.inputConfirmationCode = response.Result.ConfirmationCode;

                //if (ConfirmationCodesMatch(this.inputConfirmationCode, this.generatedConfirmationCode)) {

                //}
        }

        ///<inheritdoc/>
         public void Receive(EmailConfirmationSubmittedMessage message) {
            String inputConfirmationCode = message.ConfirmationCode;

            Debug.WriteLine($"Received confirmation code: {inputConfirmationCode}");

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
            //return true;
            return inputConfirmationCode.Equals(generatedConfirmationCode);
        }

        public string GeneratedConfirmationCode {
            get { return this.generatedConfirmationCode; }
        }

        //public string InputConfirmationCode {
        //    get { return this.inputConfirmationCode; }
        //}

        public bool IsConfirmationCodeMatch {
            get { return this.isConfirmationCodeMatch; }
            set { this.isConfirmationCodeMatch = value; }
        }
    }
}