

using AdvancedBudgetManagerCore.model.misc;

namespace AdvancedBudgetManagerCore.service {
    /// <summary>
    /// Provides functionality for displaying error messages across the application.
    /// </summary>
    public interface IErrorService {
        /// <summary>
        /// Displays a notification containing the provided error information.
        /// </summary>
        /// <param name="errorInfo">The <see cref="ErrorInfo"/> instance containing the error information.</param>
        void Notify(ErrorInfo errorInfo);
    }
}
