using AdvancedBudgetManagerCore.model.misc;

namespace AdvancedBudgetManagerCore.service {
    public interface IUserSessionService {
        AuthenticatedUser AuthenticatedUser { get; }
        bool IsAuthenticated { get; }

        void SetUser(AuthenticatedUser authenticatedUser);
        void Clear();
    }
}
