using Microsoft.UI.Xaml;

namespace AdvancedBudgetManager.utils.misc {
    public interface IWindowProvider {
        void Register(Window window);
        Window GetActiveWindow();
        XamlRoot GetActiveXamlRoot();
    }
}
