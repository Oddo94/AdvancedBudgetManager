namespace AdvancedBudgetManagerCore.model.response {
    #pragma warning disable CS1591
    public class EmailSenderCredentials {
        private string emailSenderAddress;
        private string emailSenderUserName;
        private string emailSenderPassword;

        /// <summary>
        /// Intializes a new instance of the <see cref="EmailSenderCredentials"/> class based on the provided email address, username and password.
        /// </summary>
        /// <param name="emailSenderAddress">The email address used to send the app-related email notifications</param>
        /// <param name="emailSenderUserName">The email address username</param>
        /// <param name="emailSenderPassword">The email address password</param>
        public EmailSenderCredentials(string emailSenderAddress, string emailSenderUserName, string emailSenderPassword) {
            this.emailSenderAddress = emailSenderAddress;
            this.emailSenderUserName = emailSenderUserName;
            this.emailSenderPassword = emailSenderPassword;
        }

        public string EmailSenderAddress {
            get { return this.emailSenderAddress; }
            set { this.emailSenderAddress = value; }
        }

        public string EmailSenderUserName {
            get { return this.emailSenderUserName; }
            set { this.EmailSenderUserName = value; }
        }

        public string EmailSenderPassword {
            get { return this.emailSenderPassword; }
            set { this.emailSenderPassword = value; }
        }

    }
}
