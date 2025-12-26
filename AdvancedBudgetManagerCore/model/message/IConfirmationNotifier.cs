namespace AdvancedBudgetManagerCore.model.message {
    /// <summary>
    /// Interface that specifies the notification method that needs to be implemented by all classes that want to act as notifiers.
    /// </summary>
    public interface IConfirmationNotifier {
        /// <summary>
        /// Sends the notification object to the registered object/s.
        /// </summary>
        void Notify();
    }
}
