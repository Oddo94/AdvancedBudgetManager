using AdvancedBudgetManagerCore.utils.enums;
using System.Security;

namespace AdvancedBudgetManagerCore.model.request {
    public class PasswordDataUpdateRequest : IDataUpdateRequest {
        private SecureString newPassword;

        private string userEmail;

        public PasswordDataUpdateRequest(SecureString newPassword, string userEmail) {
            this.newPassword = newPassword;
            this.userEmail = userEmail;
        }

        public DataUpdateRequestType GetDataUpdateRequestType() {
            return DataUpdateRequestType.USER_PASSWORD_UPDATE;
        }

        public string GetUpdateParameter() {
            return this.userEmail;
        }

        public SecureString NewPassword {
            get { return this.newPassword; }
        }
    }
}
