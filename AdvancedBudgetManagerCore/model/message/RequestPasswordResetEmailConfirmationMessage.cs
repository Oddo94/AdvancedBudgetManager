using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AdvancedBudgetManagerCore.model.message {
    /// <summary>
    /// Represents the message which is sent for displaying the email confirmation code pop-up during the password reset process.
    /// </summary>
    public class RequestPasswordResetEmailConfirmationMessage : RequestMessage<bool> {
    }
}
