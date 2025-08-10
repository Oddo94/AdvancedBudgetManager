using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.request {
    public class PasswordDataUpdateRequest : IDataUpdateRequest {
        private string newPassword;

        private string userEmail;

        public PasswordDataUpdateRequest(string newPassword, string userEmail) {
            this.newPassword = newPassword;
            this.userEmail = userEmail;
        }

        public DataUpdateRequestType GetDataUpdateRequestType() {
            return DataUpdateRequestType.USER_PASSWORD_UPDATE;
        }

        public string GetUpdateParameter() {
            return this.userEmail;
        }

        public string NewPassword {
            get { return this.newPassword; }
        }
    }
}
