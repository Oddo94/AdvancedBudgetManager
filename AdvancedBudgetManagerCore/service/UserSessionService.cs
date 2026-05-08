using AdvancedBudgetManagerCore.model.misc;

namespace AdvancedBudgetManagerCore.service {
    /// <summary>
    /// Service class used for performing used for performing operations related to the currently authenticated user.
    /// </summary>
    public class UserSessionService : IUserSessionService {
        /// <inheritdoc/>
        public AuthenticatedUser AuthenticatedUser { get; private set; }

        /// <inheritdoc/>
        public bool IsAuthenticated => AuthenticatedUser != null;

        /// <inheritdoc/>
        public void SetUser(AuthenticatedUser authenticatedUser) {
            AuthenticatedUser = authenticatedUser;
        }

        /// <inheritdoc/>
        public void Clear() {
            AuthenticatedUser = null;
        }
    }
}
