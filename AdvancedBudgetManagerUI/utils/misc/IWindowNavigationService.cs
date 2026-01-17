using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManager.utils.misc {
    /// <summary>
    /// Provides functionality for displaying a specific window.
    /// </summary>
    public interface IWindowNavigationService {
        /// <summary>
        /// Displays the window with the specified key.
        /// </summary>
        /// <param name="windowKey">The <see cref="WindowKey"/> enum identifying the window which needs to be displayed.</param>
        void Show(WindowKey windowKey);
    }
}