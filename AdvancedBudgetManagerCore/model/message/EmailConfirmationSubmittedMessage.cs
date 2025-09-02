using AdvancedBudgetManagerCore.model.response;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.message {
    public class EmailConfirmationSubmittedMessage : ValueChangedMessage<EmailConfirmationResponse>{
        //public string PromptTitle { get; }
        private string confirmationCode;

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
