using CommunityToolkit.Mvvm.Messaging;

namespace AdvancedBudgetManagerCore.model.message {
    public class PasswordResetEmailConfirmationNotifier : IConfirmationNotifier {
        public void Notify() {
            WeakReferenceMessenger.Default.Send(new RequestPasswordResetEmailConfirmationMessage());
        }
    }
}
