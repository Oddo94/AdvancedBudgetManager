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

        private DateTime normalizedStartDate;

        private DateTime normalizedEndDate;

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
            //DateTime startDate;
            //DateTime endDate;

            //DateTime selectedStartDate = BudgetSummaryStartDate.Date;
            //DateTime selectedEndDate = BudgetSummaryEndDate.Date;
            //if (IsMonthInterval) {
            //    startDate = new DateTime(selectedStartDate.Year, selectedStartDate.Month, 1);
            //    endDate = new DateTime(selectedEndDate.Year, selectedEndDate.Month, DateTime.DaysInMonth(selectedEndDate.Year, selectedEndDate.Month));
            //} else {
            //    startDate = new DateTime(selectedStartDate.Year, selectedStartDate.Month, 1);
            //    endDate = startDate.AddMonths(1).AddMinutes(-1);
            //    //endDate = startDate.AddDays(DateTime.DaysInMonth(selectedStartDate.Year, selectedStartDate.Month));
            //}

            DateRange? monthRange = GetMonthRange(BudgetSummaryStartDate, BudgetSummaryEndDate, IsMonthInterval);

            if (monthRange == null) {
                return;
            }

            ObservableCollection<BudgetSummaryItem> retrievedItems = new ObservableCollection<BudgetSummaryItem>();
            BudgetSummaryItems.Clear();

            BudgetSummaryDto budgetSummaryDto = budgetSummaryService.GetBudgetSummaryInfo(monthRange.StartDate, monthRange.EndDate);
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
        }

        private DateRange? GetMonthRange(DateTimeOffset? inputStartDate, DateTimeOffset? inputEndDate, bool isMonthInterval) {
            if (inputStartDate == null) {
                return null;
            }

            DateTime selectedStartDate = inputStartDate.Value.DateTime;

            DateTime normalizedStartDate;
            DateTime normalizedEndDate;
            if (isMonthInterval && inputEndDate != null) {
                DateTime selectedEndDate = inputEndDate.Value.DateTime;

                normalizedStartDate = new DateTime(selectedStartDate.Year, selectedStartDate.Month, 1);
                normalizedEndDate = new DateTime(selectedEndDate.Year, selectedEndDate.Month, DateTime.DaysInMonth(selectedEndDate.Year, selectedEndDate.Month)
                ).AddDays(1).AddTicks(-1);
            } else {
                normalizedStartDate = new DateTime(selectedStartDate.Year, selectedStartDate.Month, 1);
                normalizedEndDate = normalizedStartDate.AddMonths(1).AddTicks(-1);
            }

            return new DateRange(normalizedStartDate, normalizedEndDate);
        }


    }
}
