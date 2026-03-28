using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class BudgetSummaryViewModel : ObservableObject {
        [ObservableProperty]
        public DateTimeOffset budgetSummaryStartDate;

        [ObservableProperty]
        public DateTimeOffset budgetSummaryEndDate;

        [ObservableProperty]
        public bool isMonthInterval;

        [ObservableProperty]
        private ObservableCollection<BudgetSummaryItem> budgetSummaryItems;

        private BudgetSummaryService budgetSummaryService;

        public BudgetSummaryViewModel([NotNull] BudgetSummaryService budgetSummaryService) {
            this.budgetSummaryService = budgetSummaryService;
            this.budgetSummaryItems = new ObservableCollection<BudgetSummaryItem>();

            DateTime currentDate = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDateOfMonth = firstDateOfMonth.AddDays(DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

            this.budgetSummaryStartDate = new DateTimeOffset(firstDateOfMonth);
            this.budgetSummaryEndDate = new DateTimeOffset(lastDateOfMonth);
        }

        [RelayCommand]
        public void DisplayBudgetSummary() {
            //BudgetSummaryDto budgetSummaryDto = budgetSummaryService.GetBudgetSummaryInfo(budgetSummaryStartDate, budgetSummaryEndDate);
            DateTime startDate;
            DateTime endDate;

            DateTime selectedStartDate = BudgetSummaryStartDate.Date;
            DateTime selectedEndDate = BudgetSummaryEndDate.Date;
            if (IsMonthInterval) {
                startDate = new DateTime(selectedStartDate.Year, selectedStartDate.Month, 1);
                endDate = new DateTime(selectedEndDate.Year, selectedEndDate.Month, DateTime.DaysInMonth(selectedEndDate.Year, selectedEndDate.Month));
            } else {
                startDate = new DateTime(selectedStartDate.Year, selectedStartDate.Month, 1);
                endDate = startDate.AddMonths(1).AddMinutes(-1);
                //endDate = startDate.AddDays(DateTime.DaysInMonth(selectedStartDate.Year, selectedStartDate.Month));
            }

            ObservableCollection<BudgetSummaryItem> retrievedItems = new ObservableCollection<BudgetSummaryItem>();
            BudgetSummaryItems.Clear();

            BudgetSummaryDto budgetSummaryDto = budgetSummaryService.GetBudgetSummaryInfo(startDate, endDate);
            BudgetSummaryItem incomesItem = new BudgetSummaryItem("Incomes", budgetSummaryDto.TotalIncomes, budgetSummaryDto.TotalIncomesPercentage);
            BudgetSummaryItem expensesItem = new BudgetSummaryItem("Expenses", budgetSummaryDto.TotalExpenses, budgetSummaryDto.TotalExpensesPercentage);
            BudgetSummaryItem debtsItem = new BudgetSummaryItem("Debts", budgetSummaryDto.TotalDebts, budgetSummaryDto.TotalDebtsPercentage);
            BudgetSummaryItem savingsItem = new BudgetSummaryItem("Savings", budgetSummaryDto.TotalSavings, budgetSummaryDto.TotalSavingsPercentage);
            BudgetSummaryItem leftToSpendItem = new BudgetSummaryItem("Left to spend", budgetSummaryDto.TotalLeftToSpend, budgetSummaryDto.TotalLeftToSpendPercentage);

            BudgetSummaryItems.Add(incomesItem);
            BudgetSummaryItems.Add(expensesItem);
            BudgetSummaryItems.Add(debtsItem);
            BudgetSummaryItems.Add(savingsItem);
            BudgetSummaryItems.Add(leftToSpendItem);



            //BudgetSummaryItem incomesItem = new BudgetSummaryItem("Incomes", 10000, 100.00);
            //BudgetSummaryItem expensesItem = new BudgetSummaryItem("Expenses", 4000, 40.00);
            //BudgetSummaryItem debtsItem = new BudgetSummaryItem("Debts", 1000, 10.00);
            //BudgetSummaryItem savingsItem = new BudgetSummaryItem("Savings", 5000, 50.00);

            //BudgetSummaryItems.Add(incomesItem);
            //BudgetSummaryItems.Add(expensesItem);
            //BudgetSummaryItems.Add(debtsItem);
            //BudgetSummaryItems.Add(savingsItem);
        }
    }
}
