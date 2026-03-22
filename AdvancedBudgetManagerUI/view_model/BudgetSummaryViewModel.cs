using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManagerCore.service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class BudgetSummaryViewModel : ObservableObject {
        private DateTime budgetSummaryStartDate;

        private DateTime budgetSummaryEndDate;

        [ObservableProperty]
        private ObservableCollection<BudgetSummaryItem> budgetSummaryItems;

        private BudgetSummaryService budgetSummaryService;

        public BudgetSummaryViewModel([NotNull] BudgetSummaryService budgetSummaryService) {
            this.budgetSummaryService = budgetSummaryService;
            this.budgetSummaryItems = new ObservableCollection<BudgetSummaryItem>();
        }

        [RelayCommand]
        public void DisplayBudgetSummary() {
            //BudgetSummaryDto budgetSummaryDto = budgetSummaryService.GetBudgetSummaryInfo(budgetSummaryStartDate, budgetSummaryEndDate);           
            ObservableCollection<BudgetSummaryItem> retrievedItems = new ObservableCollection<BudgetSummaryItem>();
            BudgetSummaryItems.Clear();

            BudgetSummaryItem incomesItem = new BudgetSummaryItem("Incomes", 10000, 100.00);
            BudgetSummaryItem expensesItem = new BudgetSummaryItem("Expenses", 4000, 40.00);
            BudgetSummaryItem debtsItem = new BudgetSummaryItem("Debts", 1000, 10.00);
            BudgetSummaryItem savingsItem = new BudgetSummaryItem("Savings", 5000, 50.00);

            BudgetSummaryItems.Add(incomesItem);
            BudgetSummaryItems.Add(expensesItem);
            BudgetSummaryItems.Add(debtsItem);
            BudgetSummaryItems.Add(savingsItem);
        }
    }
}
