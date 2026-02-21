using CommunityToolkit.Mvvm.Messaging;

namespace AdvancedBudgetManagerCore.model.message {
    /// <summary>
    /// Class used for creating a <see cref="IConfirmationNotifier"/> instance which is used during the password reset process.
    /// </summary>
    public class PasswordResetEmailConfirmationNotifier : IConfirmationNotifier {
        /// <inheritdoc/>
        public void Notify() {
            WeakReferenceMessenger.Default.Send(new RequestPasswordResetEmailConfirmationMessage());
        }
    }
}
