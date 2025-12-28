using System.Security;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for retrieving a user entity.
    /// </summary>
    public class UserReadDto {
        /// <summary>
        /// The user name.
        /// </summary>
        private string userName;

        /// <summary>
        /// The users's email address.
        /// </summary>
        //private string emailAddress;

        private SecureString password;

        /// <summary>
        /// Initializes a new instance of the see <see cref="UserReadDto"/> based on the provided parameters.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The user's password.</param>
        public UserReadDto(string userName, SecureString password) {
            this.userName = userName;
            this.password = password;
        }

        public string UserName {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public SecureString Password {
            get { return this.password; }
            set { this.password = value; }
        }

    }
}
