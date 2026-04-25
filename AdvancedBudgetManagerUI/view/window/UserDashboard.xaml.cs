using AdvancedBudgetManager;
using AdvancedBudgetManager.utils.enums;
using AdvancedBudgetManager.utils.misc;
using Autofac;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManagerUI.view.window {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserDashboard : Window {
        private readonly IPageNavigationService navigationService;
        public UserDashboard() {
            this.InitializeComponent();

            AppWindow appWindow = this.AppWindow;
            if (appWindow.Presenter is OverlappedPresenter presenter) {
                presenter.Maximize();
            }

            appWindow.Hide();

            this.navigationService = App.Container.Resolve<IPageNavigationService>();
            navigationService.Initialize(this.userDashboardContentFrame);

            //Sets the default page on app startup
            navigationService.Show(PageKey.BudgetSummaryPage);
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
            if (args.SelectedItem is NavigationViewItem selectedItem) {
                String? pageName = selectedItem.Tag.ToString();

                switch (pageName) {
                    //case "incomesPage":
                    //    userDashboardContentFrame.Navigate(typeof(IncomesPage), null);
                    //    break;

                    //case "expensesPage":
                    //    userDashboardContentFrame.Navigate(typeof(ExpensesPage), null);
                    //    break;

                    case "budgetSummaryPage":
                        navigationService.Show(PageKey.BudgetSummaryPage);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
