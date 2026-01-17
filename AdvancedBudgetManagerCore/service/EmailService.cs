using AdvancedBudgetManagerCore.model;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.security;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service {
#pragma warning disable CS1591
    /// <summary>
    /// Service class used for performing the email sending operations.
    /// </summary>
    public class EmailService {
        /// <summary>
        /// The email confirmation sender instance.
        /// </summary>
        private EmailConfirmationSender emailConfirmationSender;

        /// <summary>
        /// The secret reader instance.
        /// </summary>
        private SecretReader secretReader;

        /// <summary>
        /// The generated confirmation code.
        /// </summary>
        private string generatedConfirmationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        public EmailService() {
            this.emailConfirmationSender = new EmailConfirmationSender();
            this.secretReader = new SecretReader();
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
            }

            return emailSendingResult;
        }

        /// <summary>
        /// Retrieves the email action to be displayed inside the sent email.
        /// </summary>
        /// <param name="emailPurpose">The <see cref="EmailPurpose"/> enum value used for specifying the email purpose.</param>
        /// <returns></returns>
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
