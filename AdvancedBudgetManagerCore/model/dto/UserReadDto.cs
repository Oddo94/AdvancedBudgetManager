namespace AdvancedBudgetManagerCore.model.dto {
    public class UserReadDto {
        private string userName;
        private string emailAddress;

        public UserReadDto(string userName, string emailAddress) {
            this.userName = userName;
            this.emailAddress = emailAddress;
        }

        public string UserName {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public string EmailAddress {
            get { return this.emailAddress; }
            set { this.emailAddress = value; }
        }

    }
}
