using AdvancedBudgetManagerCore.model;
using AdvancedBudgetManagerCore.model.response;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace AdvancedBudgetManagerCore.utils.security {
    /// <summary>
    /// Utility class for performing email confirmation related operations.
    /// </summary>
    public class EmailConfirmationSender {

        /// <summary>
        /// Sends the confirmation email to the specified email address.
        /// </summary>
        /// <param name="senderCredentials">Object that contains the details of the sender email address.</param>
        /// <param name="confirmationEmailDetails">Object that contains the details related to the recipient.</param>
        /// <exception cref="SystemException"></exception>
        public void SendConfirmationEmail(EmailSenderCredentials senderCredentials, ConfirmationEmailDetails confirmationEmailDetails) {
            try {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(senderCredentials.EmailSenderAddress);
                mail.To.Add(confirmationEmailDetails.RecipientEmailAddress);
                mail.Subject = confirmationEmailDetails.EmailSubject;
                mail.Body = String.Format(confirmationEmailDetails.EmailBody, confirmationEmailDetails.ConfirmationCode);

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(senderCredentials.EmailSenderUserName, senderCredentials.EmailSenderPassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);//ONLY FOR TESTING PURPOSES!
            } catch (Exception ex) {
                throw new SystemException($"An error occurred while sending the confirmation code by email. Reason: {ex.Message}");
            }

        }

        /// <summary>
        /// Generates a new confirmation code of specified size.
        /// </summary>
        /// <param name="codeSize">The size of the confirmation code.</param>
        /// <returns>The confirmation code represented as a <see cref="String"/> object.</returns>
        /// <exception cref="ArgumentException"></exception>
        public String GenerateConfirmationCode(int codeSize) {
            if (codeSize <= 0) {
                throw new ArgumentException("The specified confirmation code size must be greater than 0!");
            }

            RandomNumberGenerator crypto = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[codeSize];
            crypto.GetNonZeroBytes(randomBytes);

            String confirmationCode = ConvertBinaryToHex(randomBytes);

            return confirmationCode;
        }

        /// <summary>
        /// Converts the specified <see cref="byte"/> array tot hexadecimal format.
        /// </summary>
        /// <param name="inputArray">The input represented as a <see cref="byte"/> array object.</param>
        /// <returns>The <see cref="String"/> representation of the input.</returns>
        private String ConvertBinaryToHex([DisallowNull] byte[] inputArray) {
            StringBuilder resultArray = new StringBuilder(inputArray.Length * 2);

            foreach (byte currentByte in inputArray) {
                resultArray.AppendFormat("{0:x2}", currentByte);
            }

            return resultArray.ToString();
        }


        //public bool ConfirmationCodesMatch([DisallowNull] string generatedConfirmationCode, [DisallowNull] string userInputConfirmationCode) {
        //    return true;
        //    //return generatedConfirmationCode.Equals(userInputConfirmationCode);
        //}
    }
}
