

using AdvancedBudgetManagerCore.model.misc;

namespace AdvancedBudgetManagerCore.service {
    public interface IErrorService {
        void Notify(ErrorInfo errorInfo);
    }
}
