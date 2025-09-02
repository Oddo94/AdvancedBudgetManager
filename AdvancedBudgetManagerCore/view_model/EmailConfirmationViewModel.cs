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
    public partial class EmailConfirmationViewModel : ObservableObject, IRecipient<EmailConfirmationSubmittedMessage> {
        [ObservableProperty]
        private string userEmail;

        //[ObservableProperty]
        //private string inputConfirmationCode;

        private ICrudRepository emailConfirmationRepository;
        private EmailConfirmationSender emailConfirmationSender;
        private SecretReader secretReader;
        private string generatedConfirmationCode;


        public EmailConfirmationViewModel() {
            //this.emailConfirmationRepository = emailConfirmationRepository;
            this.emailConfirmationSender = new EmailConfirmationSender();
            this.secretReader = new SecretReader();
            //IsActive = true;
            WeakReferenceMessenger.Default.Register<EmailConfirmationSubmittedMessage>(this);

        }

        [RelayCommand]
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

            WeakReferenceMessenger.Default.Send(new RequestEmailConfirmationMessage());

            //if (message.HasReceivedResponse) {
            //    Task<EmailConfirmationResponse> response = message.Response;

            //    this.inputConfirmationCode = response.Result.ConfirmationCode;

                //if (ConfirmationCodesMatch(this.inputConfirmationCode, this.generatedConfirmationCode)) {

                //}
        }

         public void Receive(EmailConfirmationSubmittedMessage message) {
            String confirmationCode = message.ConfirmationCode;

            Debug.WriteLine($"Received confirmation code: {confirmationCode}");
        }

        //[RelayCommand]
        public bool ConfirmationCodesMatch([NotNull] string inputConfirmationCode, [NotNull] string generatedConfirmationCode) {
            return true;
            //return inputConfirmationCode.Equals(generatedConfirmationCode);
        }

        public string GeneratedConfirmationCode {
            get { return this.generatedConfirmationCode; }
        }

        //public string InputConfirmationCode {
        //    get { return this.inputConfirmationCode; }
        //}
    }
}