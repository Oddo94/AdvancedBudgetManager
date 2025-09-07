using AdvancedBudgetManagerCore.model.response;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AdvancedBudgetManagerCore.model.message {
    public class ResetPasswordSubmittedMessage : ValueChangedMessage<ResetPasswordResponse> {
        private ResetPasswordResponse resetPasswordResponse;

        public ResetPasswordSubmittedMessage(ResetPasswordResponse resetPasswordResponse) : base(resetPasswordResponse) {
            this.resetPasswordResponse = resetPasswordResponse;
        }

        public ResetPasswordResponse ResetPasswordResponse {
            get { return this.resetPasswordResponse; }
            set { this.resetPasswordResponse = value; }
        }
    }
}
