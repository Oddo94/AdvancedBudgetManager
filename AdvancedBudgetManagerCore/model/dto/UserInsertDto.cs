using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.dto {
    public class UserInsertDto {
        private String userName;
        //private byte[] salt;
        private SecureString password;
        private String emailAddress;

        public UserInsertDto() { }

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

        //public byte[] Salt {
        //    get { return this.salt; }
        //    set { this.salt = value; }
        //}

        public SecureString Password{
            get { return this.password; }
            set { this.password = value; }
        }

        public String EmailAddress {
            get { return this.emailAddress; }
            set { this.emailAddress = value; }
        }
    }
   
}
