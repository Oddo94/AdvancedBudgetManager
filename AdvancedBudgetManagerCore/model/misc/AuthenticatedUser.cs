namespace AdvancedBudgetManagerCore.model.misc {
    public class AuthenticatedUser {
        private long userId;
        private string emailAddress;

        public AuthenticatedUser(long userId, string emailAddress) {
            this.userId = userId;
            this.emailAddress = emailAddress;
        }

        public long UserId {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public string EmailAddress {
            get { return this.emailAddress; }
            set { this.emailAddress = value; }
        }
    }
}
