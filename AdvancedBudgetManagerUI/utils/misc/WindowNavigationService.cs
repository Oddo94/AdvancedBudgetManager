using AdvancedBudgetManagerCore.utils.enums;
using Autofac;
using Microsoft.UI.Xaml;

namespace AdvancedBudgetManager.utils.misc {
    /// <summary>
    /// Service class which allows the display of a specified window.
    /// </summary>
    public class WindowNavigationService : IWindowNavigationService {
        /// <summary>
        /// The scope used for creating the specified window.
        /// </summary>
        private ILifetimeScope rootScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowNavigationService"/> class.
        /// </summary>
        /// <param name="rootScope">The <see cref="ILifetimeScope"/> instance used as a root scope.</param>
        public WindowNavigationService(ILifetimeScope rootScope) {
            this.rootScope = rootScope;
        }

        /// <inheritdoc/>
        public void Show(WindowKey windowKey) {
            ILifetimeScope scope = rootScope.BeginLifetimeScope();
            Window window = scope.ResolveKeyed<Window>(windowKey);

            window.Closed += (_, _) => scope.Dispose();
            window.Activate();
        }
    }
}
