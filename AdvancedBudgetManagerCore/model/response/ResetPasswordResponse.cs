namespace AdvancedBudgetManagerCore.model.response {
    public class ResetPasswordResponse {
        private string newPassword;
        private string confirmationPassword;

        public ResetPasswordResponse(string newPassword, string confirmationPassword) {
            this.newPassword = newPassword;
            this.confirmationPassword = confirmationPassword;
        }

        public string NewPassword {
            get { return this.confirmationPassword; }
            set { this.newPassword = value; }
        }

        public string ConfirmationPassword {
            get { return this.confirmationPassword; }
            set { this.confirmationPassword = value; }
        }

    }
}
