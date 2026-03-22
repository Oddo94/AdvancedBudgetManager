using AdvancedBudgetManagerCore.view_model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.page {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudgetSummaryPage : Page {
        private BudgetSummaryViewModel budgetSummaryViewModel;
        //public BudgetSummaryPage([NotNull] BudgetSummaryViewModel budgetSummaryViewModel) {
        //    InitializeComponent();
        //    this.budgetSummaryViewModel = budgetSummaryViewModel;
        //    //budgetSummaryItems = new ObservableCollection<BudgetSummaryItem>();
        //}

        public BudgetSummaryPage(BudgetSummaryViewModel budgetSummaryViewModel) {
            InitializeComponent();
            this.budgetSummaryViewModel = budgetSummaryViewModel;
        }

        public void Button_Click(object sender, RoutedEventArgs e) {
            //Console.WriteLine("Inside 'Button_Click' method");
            //if ("This is a test message".Equals(sampleTextBlock.Text)) {
            //    sampleTextBlock.Text = "You clicked the button!";
            //} else {
            //    sampleTextBlock.Text = "This is a test message";
            //}

            //coreModelWrapper.XAxis.Clear();
            //coreModelWrapper.XAxis.Add(new Axis() {
            //    Labels = new string[] {
            //                    "JAN",
            //                    "FEB",
            //                    "MAR",
            //                    "APR",
            //                    "MAY",
            //                    "JUN",
            //                    "JUL",
            //                    "AUG",
            //                    "SEP",
            //                    "OCT",
            //                    "NOV",
            //                    "DEC"
            //                }
            //});
        }

        public void LowercaseButton_Click(object sender, RoutedEventArgs e) {
            //ICartesianAxis newAxis = new Axis() {
            //    Labels = new string[] {
            //                    "Jan",
            //                    "Feb",
            //                    "Mar",
            //                    "Apr",
            //                    "May",
            //                    "Jun",
            //                    "Jul",
            //                    "Aug",
            //                    "Sep",
            //                    "Oct",
            //                    "Nov",
            //                    "Dec"
            //                }
            //};

            //setAxis(coreModelWrapper.XAxis, newAxis);
        }

        public void ChangeChartDataButton_Click(object sender, RoutedEventArgs e) {
            //coreViewModel.updateData();

            //ObservableCollection<DataPoint> updatedCollection = coreViewModel.Series;
            //List<double> seriesList = new List<double>();

            //foreach (DataPoint currentPoint in updatedCollection) {
            //    seriesList.Add(currentPoint.Value);
            //}

            //coreModelWrapper.Series.Clear();
            //coreModelWrapper.Series.Add(new ColumnSeries<double>(values: seriesList.ToArray()));
        }

    }
}
