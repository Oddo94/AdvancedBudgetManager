using AdvancedBudgetManagerCore.model.response;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.message {
    #pragma warning disable CS1591
    /// <summary>
    /// Represents the object that stores the confirmation code submitted by the user.
    /// </summary>
    public class EmailConfirmationSubmittedMessage : ValueChangedMessage<EmailConfirmationResponse>{
        //public string PromptTitle { get; }
        private string confirmationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailConfirmationSubmittedMessage"/> based on the provided confirmation code.
        /// </summary>
        /// <param name="emailConfirmationResponse"></param>
        public EmailConfirmationSubmittedMessage(EmailConfirmationResponse emailConfirmationResponse) : base(emailConfirmationResponse) {
            //PromptTitle = "Email confirmation code";
            this.confirmationCode = emailConfirmationResponse.ConfirmationCode;
        }

        public string ConfirmationCode {
            get { return this.confirmationCode; }
            set { this.confirmationCode = value; }
        }
    }
}
