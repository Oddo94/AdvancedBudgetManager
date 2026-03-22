using AdvancedBudgetManager.utils.enums;
using Microsoft.UI.Xaml.Controls;

namespace AdvancedBudgetManager.utils.misc {
    public interface IPageNavigationService {
        void Initialize(Frame frame);
        void Show(PageKey pageKey);
    }
}
