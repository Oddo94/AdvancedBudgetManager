using AdvancedBudgetManagerCore.model.misc;

namespace AdvancedBudgetManagerCore.service {
    /// <summary>
    /// Interface that specifies the operations that should be implemented by a user session service. 
    /// </summary>
    public interface IUserSessionService {
        /// <summary>
        /// The currently authenticated user.
        /// </summary>
        AuthenticatedUser AuthenticatedUser { get; }

        /// <summary>
        /// Property that shows if the user is authenticated or not.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Sets the currently authenticated user.
        /// </summary>
        /// <param name="authenticatedUser">An <see cref="AuthenticatedUser"/> object containing the user data.</param>
        void SetUser(AuthenticatedUser authenticatedUser);

        /// <summary>
        /// Clears the currently authenticated user.
        /// </summary>
        void Clear();
    }
}
