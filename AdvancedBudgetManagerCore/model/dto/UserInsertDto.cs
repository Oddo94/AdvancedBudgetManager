using System;
using System.Security;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for sending the information related to user insertion.
    /// </summary>
    public class UserInsertDto {
        /// <summary>
        /// The user name.
        /// </summary>
        private String userName;

        /// <summary>
        /// The password.
        /// </summary>
        private SecureString password;

        /// <summary>
        /// The users's email address.
        /// </summary>
        private String emailAddress;

        /// <summary>
        /// Default no-args constructor
        /// </summary>
        public UserInsertDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInsertDto"/> based on the provided parameters.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="emailAddress">The users's email address.</param>
        public UserInsertDto(String userName, SecureString password, String emailAddress) {
            this.userName = userName;
            //this.salt = salt;
            this.password = password;
            this.emailAddress = emailAddress;
        }

        public String UserName {
            get { return this.userName; }
            set { this.userName = value; }
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
