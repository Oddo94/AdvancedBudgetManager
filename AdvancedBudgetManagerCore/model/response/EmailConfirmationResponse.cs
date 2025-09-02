using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.response {
    public class EmailConfirmationResponse {
        private string confirmationCode;

        public EmailConfirmationResponse(string confirmationCode) {
            this.confirmationCode = confirmationCode;
        }

        public String ConfirmationCode {
            get { return this.confirmationCode; }
            set { this.confirmationCode = value; }
        }

    }
}
