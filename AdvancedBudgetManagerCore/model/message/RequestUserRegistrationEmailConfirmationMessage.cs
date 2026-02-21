using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AdvancedBudgetManagerCore.model.message {
    /// <summary>
    /// Class used for creating the email confirmation object sent during the user registration process.
    /// </summary>
    public class RequestUserRegistrationEmailConfirmationMessage : RequestMessage<bool> {
    }
}
