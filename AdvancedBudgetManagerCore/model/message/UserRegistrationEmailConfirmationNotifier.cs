using CommunityToolkit.Mvvm.Messaging;

namespace AdvancedBudgetManagerCore.model.message {
    public class UserRegistrationEmailConfirmationNotifier : IConfirmationNotifier {
        public void Notify() {
            WeakReferenceMessenger.Default.Send(new RequestUserRegistrationEmailConfirmationMessage());
        }
    }
}
