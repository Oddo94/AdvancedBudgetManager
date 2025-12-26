using System;

namespace AdvancedBudgetManagerCore.model.entity {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the user entity.
    /// </summary>
    public class User {
        /// <summary>
        /// The user id.
        /// </summary>
        private long? userId;

        /// <summary>
        /// The user name.
        /// </summary>
        private String userName;

        /// <summary>
        /// The salt.
        /// </summary>
        private byte[] salt;

        /// <summary>
        /// The passsword hash.
        /// </summary>
        private String passwordHash;

        /// <summary>
        /// The users's email address.
        /// </summary>
        private String emailAddress;

        /// <summary>
        /// Default no-args constructor.
        /// </summary>
        public User() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> entity based on the provided parameters.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="passwordHash">The password hash.</param>
        /// <param name="emailAddress">The user's email address.</param>
        public User(long? userId, String userName, byte[] salt, String passwordHash, String emailAddress) {
            this.userId = userId;
            this.userName = userName;
            this.salt = salt;
            this.passwordHash = passwordHash;
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

        public String PasswordHash {
            get { return this.passwordHash; }
            set { this.passwordHash = value; }
        }

        public String EmailAddress {
            get { return this.emailAddress; }
            set { this.emailAddress = value; }
        }
    }
}
