using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManager.view_model;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.page {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IncomesPage : Page {
        public IncomesViewModel incomesViewModel;
        private ContentDialog incomesInfoDialog;
        private DialogService dialogService;

        public IncomesPage([NotNull] IncomesViewModel incomesViewModel) {
            this.incomesViewModel = incomesViewModel;
            //this.incomesInfoDialog = new ContentDialog {
            //    Title = "Incomes",
            //    Content = "No income data found for the specified time interval.",
            //    CloseButtonText = "Ok",
            //    XamlRoot = this.XamlRoot
            //};
            //this.incomesInfoDialog = new ContentDialog();
            //this.incomesViewModel.NoGeneralIncomeDataFound += IncomesViewModel_NoGeneralIncomeDataFound;
            //this.incomesViewModel.NoMonthlyEvolutionDataFound += IncomesViewModel_NoMonthlyEvolutionDataFound;
            //this.incomesViewModel.PropertyChanged += IncomesViewModel_HasMonthlyEvolutionData;

            InitializeComponent();
            this.dialogService = new DialogService();
        }

        private async void IncomesViewModel_NoGeneralIncomeDataFound(object? sender, EventArgs e) {
            //incomesInfoDialog = new ContentDialog {
            //    Title = "Incomes",
            //    Content = ((CustomEventArgs)e).Message,
            //    CloseButtonText = "Ok",
            //    XamlRoot = this.XamlRoot
            //};

            //await incomesInfoDialog.ShowAsync();
            dialogService.XamlRoot = this.XamlRoot;

            string message = ((CustomEventArgs)e).Message;
            string title = "Incomes";


            await dialogService.ShowAsync(message, title);
        }

        private async void IncomesViewModel_NoMonthlyEvolutionDataFound(object? sender, EventArgs e) {
            //ContentDialog incomesInfoDialog2 = new ContentDialog {
            //    Title = "Incomes",
            //    Content = "No monthly income evolution data was found for the specified year.",
            //    CloseButtonText = "Ok",
            //    XamlRoot = this.XamlRoot
            //};

            //await incomesInfoDialog2.ShowAsync();
            //Debug.WriteLine("HAS CURRENT THREAD ACCESS: " + this.DispatcherQueue.HasThreadAccess);

            dialogService.XamlRoot = this.XamlRoot;

            string message = ((CustomEventArgs)e).Message;
            string title = "Incomes";

            //Debug.WriteLine("IS LOADED:" + this.IsLoaded);
            //Debug.WriteLine("IS XAML ROOT NOT NULL:" + (this.XamlRoot != null));
            //Debug.WriteLine("IS CONTENT.XAML_ROOT NOT NULL:" + (this.Content?.XamlRoot != null));

            //DispatcherQueue.TryEnqueue(async () => {
            await dialogService.ShowAsync(message, title);
            //});
        }

        private async void IncomesViewModel_HasMonthlyEvolutionData(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(IncomesViewModel.HasMonthlyEvolutionData)) {
                if (!incomesViewModel.HasMonthlyEvolutionData) {
                    // IMPORTANT: wait one UI cycle so LiveCharts finishes rendering
                    await DispatcherQueue.EnqueueAsync(async () => {
                        await Task.Yield();

                        await dialogService.ShowAsync(
                            "No monthly income evolution data was found for the specified year.",
                            "Incomes");
                    });

                    incomesViewModel.HasMonthlyEvolutionData = false; // reset
                }
            }
        }

        //public async void DisplayIncomes_Click(object sender, RoutedEventArgs e) {
        //    if (!incomesViewModel.HasGeneralIncomeData) {
        //        this.incomesInfoDialog = new ContentDialog {
        //            Title = "Incomes",
        //            Content = "No income data found for the specified time interval.",
        //            CloseButtonText = "Ok",
        //            XamlRoot = this.XamlRoot
        //        };

        //        await incomesInfoDialog.ShowAsync();
        //    }
        //}

        //public async void DisplayIncomeEvolutionButton_Click(object sender, RoutedEventArgs e) {
        //    if (!incomesViewModel.HasMonthlyEvolutionData) {
        //        await incomesInfoDialog.ShowAsync();
        //    }
        //}
    }
}
