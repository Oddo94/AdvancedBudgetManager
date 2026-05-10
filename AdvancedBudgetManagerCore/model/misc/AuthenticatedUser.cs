namespace AdvancedBudgetManagerCore.model.misc {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the object used to store information about the currently authenticated user.
    /// </summary>
    public class AuthenticatedUser {
        /// <summary>
        /// The user id.
        /// </summary>
        private long userId;

        /// <summary>
        /// The email address.
        /// </summary>
        private string emailAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedUser"/> object based on the provided parameters.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="emailAddress">The email address.</param>
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
