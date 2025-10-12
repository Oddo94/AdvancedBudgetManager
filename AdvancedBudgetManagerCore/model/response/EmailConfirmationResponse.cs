using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.response {
    #pragma warning disable CS1591 
    /// <summary>
    /// DTO class used to store the confirmation code submitted by the user.
    /// </summary>
    public class EmailConfirmationResponse {
        /// <summary>
        /// The confirmation code.
        /// </summary>
        private string confirmationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailConfirmationResponse"/> based on the provided confirmation code.
        /// </summary>
        /// <param name="confirmationCode">The confirmation code.</param>
        public EmailConfirmationResponse(string confirmationCode) {
            this.confirmationCode = confirmationCode;
        }

        public String ConfirmationCode {
            get { return this.confirmationCode; }
            set { this.confirmationCode = value; }
        }

    }
}
