using AdvancedBudgetManager;
using AdvancedBudgetManager.utils.enums;
using AdvancedBudgetManager.utils.misc;
using Autofac;
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

            //if (userDashboardContentFrame == null) {
            //    throw new Exception("Frame is NULL after InitializeComponent");
            //}
            //this.contentLoaded += UserDashboard_Loaded; 

            this.navigationService = App.Container.Resolve<IPageNavigationService>();
            navigationService.Initialize(this.userDashboardContentFrame);
        }

        //public void UserDashboard_Loaded(object sender, RoutedEventArgs e) {
        //    userDashboardContentFrame.Navigate(typeof(BudgetSummaryPage));
        //}

        //private void MyButton_Click(object sender, RoutedEventArgs e)
        //{
        //    MyButton.Content = "Clicked";
        //}

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
                        //userDashboardContentFrame.Navigate(typeof(BudgetSummaryPage), null);
                        navigationService.Show(PageKey.BudgetSummaryPage);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
