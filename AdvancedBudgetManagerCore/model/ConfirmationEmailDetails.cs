namespace AdvancedBudgetManagerCore.model {
    #pragma warning disable CS1591
    /// <summary>
    /// Represents the object that is used to store the confirmation email details.
    /// </summary>
    public class ConfirmationEmailDetails {
        private string recipientEmailAddress;
        private string emailSubject;
        private string emailBody;
        private string confirmationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationEmailDetails"/> based on the recipient email address, email subject, email body and confirmation code>
        /// </summary>
        /// <param name="recipientEmailAddress">The email address of the recipient</param>
        /// <param name="emailSubject">The email subject</param>
        /// <param name="emailBody">The email body</param>
        /// <param name="confirmationCode">The confirmation code</param>
        public ConfirmationEmailDetails(string recipientEmailAddress, string emailSubject, string emailBody, string confirmationCode) {
            this.recipientEmailAddress = recipientEmailAddress;
            this.emailSubject = emailSubject;
            this.emailBody = emailBody;
            this.confirmationCode = confirmationCode;
        }

        public string RecipientEmailAddress {
            get { return this.recipientEmailAddress; }
            set { this.recipientEmailAddress = value; }
        }

        public string EmailSubject {
            get { return this.emailSubject; }
            set { this.emailSubject = value; }
        }

        public string EmailBody {
            get { return this.emailBody; }
            set { this.emailBody = value; }
        }

        public string ConfirmationCode {
            get { return this.confirmationCode; }
            set { this.confirmationCode = value; }
        }
    }
}
