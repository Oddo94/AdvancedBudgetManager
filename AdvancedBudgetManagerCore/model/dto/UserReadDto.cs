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
        private string emailAddress;

        /// <summary>
        /// Initializes a new instance of the see <see cref="UserReadDto"/> based on the provided parameters.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="emailAddress">The user's email address.</param>
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
