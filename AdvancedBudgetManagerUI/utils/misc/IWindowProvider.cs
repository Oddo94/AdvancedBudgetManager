using Microsoft.UI.Xaml;

namespace AdvancedBudgetManager.utils.misc {
    /// <summary>
    /// Provides functionality for keeping track of the currently active window.
    /// </summary>
    public interface IWindowProvider {
        /// <summary>
        /// Registers a new window to the window storage collection.
        /// </summary>
        /// <param name="window">The <see cref="Window"/> instance to be registered.</param>
        void Register(Window window);

        /// <summary>
        /// Returns the currently active window.
        /// </summary>
        /// <returns>The active <see cref="Window"/> instance.</returns>
        Window GetActiveWindow();

        /// <summary>
        /// Returns the XamlRoot of the currently active window
        /// </summary>
        /// <returns>The <see cref="XamlRoot"/> of the active window.</returns>
        XamlRoot GetActiveXamlRoot();
    }
}
