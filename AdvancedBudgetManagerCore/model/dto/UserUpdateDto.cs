using System;

namespace AdvancedBudgetManagerCore.model.dto {
    public class UserUpdateDto {
        private long userId;
        private String userName;
        private byte[] salt;
        private String passwordHash;
        private String emailAddress;

        public UserUpdateDto() { }

        public UserUpdateDto(long userId, String userName, byte[] salt, String passwordHash, String emailAddress) {
            this.userId = userId;
            this.userName = userName;
            this.salt = salt;
            this.passwordHash = passwordHash;
            this.emailAddress = emailAddress;
        }

        public long UserId {
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
