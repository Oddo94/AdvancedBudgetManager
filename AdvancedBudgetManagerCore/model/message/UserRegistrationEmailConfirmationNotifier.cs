using CommunityToolkit.Mvvm.Messaging;

namespace AdvancedBudgetManagerCore.model.message {
    /// <summary>
    /// Class used for creating objects that send a UI notification for providing the email confirmation code.
    /// </summary>
    public class UserRegistrationEmailConfirmationNotifier : IConfirmationNotifier {

        /// <inheritdoc/>
        public void Notify() {
            WeakReferenceMessenger.Default.Send(new RequestUserRegistrationEmailConfirmationMessage());
        }
    }
}
