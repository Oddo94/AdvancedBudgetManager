using AdvancedBudgetManagerCore.model;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.security;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service {
    public class EmailService {
        private EmailConfirmationSender emailConfirmationSender;
        //private IConfirmationNotifier emailConfirmationNotifier;
        private SecretReader secretReader;
        private string generatedConfirmationCode;
        //private bool isConfirmationCodeMatch;

        public EmailService() {
            //this.emailConfirmationNotifier = emailConfirmationNotifier;
            this.emailConfirmationSender = new EmailConfirmationSender();
            this.secretReader = new SecretReader();

            //isConfirmationCodeMatch = true;
            //WeakReferenceMessenger.Default.Register<EmailConfirmationSubmittedMessage>(this);
        }

        /// <summary>
        /// Sends the confirmation code to the user email address.
        /// </summary>
        /// <exception cref="SystemException"></exception>
        public GenericResponse SendEmail([NotNull] string recipientEmailAddress, EmailPurpose emailPurpose) {
            if (recipientEmailAddress == null) {
                throw new ArgumentException("The recipient's email address must not be null.");
            }

            GenericResponse emailSendingResult;
            EmailSenderCredentials emailSenderCredentials = secretReader.GetEmailSenderCredentials();

            int confirmationCodeSize = 32;
            generatedConfirmationCode = emailConfirmationSender.GenerateConfirmationCode(confirmationCodeSize);

            string emailAction = GetEmailAction(emailPurpose);
            string emailSubject = $"BudgetManager-{emailAction}";
            string emailBody = $"A {emailAction} was requested for the BudgetManager application account associated to this email address.\nPlease enter the following code to finish the {emailAction} process: {generatedConfirmationCode} \nIf you have not requested the {emailAction} please ignore this email and delete it immediately.";

            ConfirmationEmailDetails confirmationEmailDetails = new ConfirmationEmailDetails(recipientEmailAddress, emailSubject, emailBody);

            try {
                emailConfirmationSender.SendConfirmationEmail(emailSenderCredentials, confirmationEmailDetails);
                emailSendingResult = new GenericResponse(ResultCode.OK, $"The {emailAction} email was succesfully sent.");
            } catch (Exception) {
                emailSendingResult = new GenericResponse(ResultCode.ERROR, $"An error occurred while sending the {emailAction} email");
                //throw new SystemException("An error occurred while sending the confirmation code by email");
            }

            return emailSendingResult;
        }

        private string GetEmailAction(EmailPurpose emailPurpose) {
            string emailAction = string.Empty;

            switch (emailPurpose) {
                case EmailPurpose.REGISTER_USER_EMAIL:
                    emailAction = "user registration";
                    break;

                case EmailPurpose.RESET_PASSWORD_EMAIL:
                    emailAction = "password reset";
                    break;

                default:
                    emailAction = "[undefined]";
                    break;
            }

            return emailAction;
        }

        public String GeneratedConfirmationCode {
            get { return this.generatedConfirmationCode; }
        }
    }
}
