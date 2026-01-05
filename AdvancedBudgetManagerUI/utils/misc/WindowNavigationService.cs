using AdvancedBudgetManagerCore.utils.enums;
using Autofac;
using Microsoft.UI.Xaml;

namespace AdvancedBudgetManager.utils.misc {
    public class WindowNavigationService : IWindowNavigationService {
        private ILifetimeScope rootScope;

        public WindowNavigationService(ILifetimeScope rootScope) {
            this.rootScope = rootScope;
        }

        public void Show(WindowKey windowKey) {
            ILifetimeScope scope = rootScope.BeginLifetimeScope();
            Window window = scope.ResolveKeyed<Window>(windowKey);

            window.Closed += (_, _) => scope.Dispose();
            window.Activate();
        }
    }
}
