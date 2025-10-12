using AdvancedBudgetManagerCore.utils.enums;
using System.Security;

namespace AdvancedBudgetManagerCore.model.request {
    /// <summary>
    /// Represents the object used to store the data required for performing the password reset.
    /// </summary>
    public class PasswordDataUpdateRequest : IDataUpdateRequest {
        /// <summary>
        /// The new password.
        /// </summary>
        private SecureString newPassword;

        /// <summary>
        /// The user email.
        /// </summary>
        private string userEmail;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordDataUpdateRequest"/> based on the new password and the user email.
        /// </summary>
        /// <param name="newPassword">The new password</param>
        /// <param name="userEmail">The user email</param>
        public PasswordDataUpdateRequest(SecureString newPassword, string userEmail) {
            this.newPassword = newPassword;
            this.userEmail = userEmail;
        }

        /// <summary>
        /// Retrieves 
        /// </summary>
        /// <returns></returns>
        public DataUpdateRequestType GetDataUpdateRequestType() {
            return DataUpdateRequestType.USER_PASSWORD_UPDATE;
        }

        ///<inheritdoc/>
        public string GetUpdateParameter() {
            return this.userEmail;
        }

        ///<inheritdoc/>
        public SecureString NewPassword {
            get { return this.newPassword; }
        }
    }
}
