namespace AdvancedBudgetManagerCore.model {
    public class ConfirmationEmailDetails {
        private string recipientEmailAddress;
        private string emailSubject;
        private string emailBody;
        private string confirmationCode;

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
