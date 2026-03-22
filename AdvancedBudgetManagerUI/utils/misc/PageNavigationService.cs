using AdvancedBudgetManager.utils.enums;
using Autofac;
using Microsoft.UI.Xaml.Controls;

namespace AdvancedBudgetManager.utils.misc {
    public class PageNavigationService : IPageNavigationService {
        private Frame frame;
        private readonly IContainer container;
        private ILifetimeScope rootScope;

        //public PageNavigationService(IContainer container) {
        //    this.container = container;
        //}

        public PageNavigationService(ILifetimeScope rootScope) {
            this.rootScope = rootScope;
        }

        public void Initialize(Frame frame) {
            this.frame = frame;
        }

        public void Show(PageKey pageKey) {
            ILifetimeScope scope = rootScope.BeginLifetimeScope();
            Page page = scope.ResolveKeyed<Page>(pageKey);

            frame.Content = page;
        }
    }
}
