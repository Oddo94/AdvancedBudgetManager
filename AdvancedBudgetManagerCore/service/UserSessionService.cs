using AdvancedBudgetManagerCore.model.misc;

namespace AdvancedBudgetManagerCore.service {
    public class UserSessionService : IUserSessionService {
        public AuthenticatedUser AuthenticatedUser { get; private set; }

        public bool IsAuthenticated => AuthenticatedUser != null;

        public void SetUser(AuthenticatedUser authenticatedUser) {
            AuthenticatedUser = authenticatedUser;
        }

        public void Clear() {
            AuthenticatedUser = null;
        }
    }
}
