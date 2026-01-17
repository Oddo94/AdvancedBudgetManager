using System;
using System.Security;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for sending the information related to user update.
    /// </summary>
    public class UserUpdateDto {
        /// <summary>
        /// The user id.
        /// </summary>
        private long? userId;

        /// <summary>
        /// The user name.
        /// </summary>
        private String userName;

        /// <summary>
        /// The salt byte array.
        /// </summary>
        private byte[] salt;

        /// <summary>
        /// The password.
        /// </summary>
        private SecureString password;

        /// <summary>
        /// The users's email address.
        /// </summary>
        private String emailAddress;

        /// <summary>
        /// Default no-args constructor.
        /// </summary>
        public UserUpdateDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUpdateDto"/> based on the provided parameters.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="salt">The <see cref="byte"/> array containing the salt.</param>
        /// <param name="password">The password represented as a <see cref="SecureString"/> instance.</param>
        /// <param name="emailAddress">The user's email address.</param>
        public UserUpdateDto(long? userId, String userName, byte[] salt, SecureString password, String emailAddress) {
            this.userId = userId;
            this.userName = userName;
            this.salt = salt;
            this.password = password;
            this.emailAddress = emailAddress;
        }

        public long? UserId {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public String UserName {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public byte[] Salt {
            get { return this.salt; }
            set { this.salt = value; }
        }

        public SecureString Password {
            get { return this.password; }
            set { this.password = value; }
        }

        public String EmailAddress {
            get { return this.emailAddress; }
            set { this.emailAddress = value; }
        }
    }
}
