using AdvancedBudgetManager.utils;
using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class BudgetSummaryViewModel : ObservableValidator {
        [ObservableProperty]
        //[DateRange("StartDate", "EndDate")]
        public DateTimeOffset startDate;

        [ObservableProperty]
        public DateTimeOffset endDate;

        [ObservableProperty]
        public bool isMonthInterval;

        [ObservableProperty]
        private ObservableCollection<BudgetSummaryItem> budgetSummaryItems;

        [ObservableProperty]
        public bool isValidDateSelection;

        private DateTime normalizedStartDate;

        private DateTime normalizedEndDate;

        private BudgetSummaryService budgetSummaryService;

        private DateTimeUtils dateTimeUtils;

        private InputDataValidator dataValidator;

        //public string StartDateError => GetErrors(nameof(StartDate))?.Cast<string>().FirstOrDefault();
        //public string StartDateError => "This is a test error for the start date picker";

        //public string EndDateError => GetErrors(nameof(EndDate))?.Cast<string>().FirstOrDefault();

        //public string EndDateError => "This is a test error for the end date picker";



        public BudgetSummaryViewModel([NotNull] BudgetSummaryService budgetSummaryService, [NotNull] DateTimeUtils dateTimeUtils, [NotNull] InputDataValidator dataValidator) {
            this.budgetSummaryService = budgetSummaryService;
            this.dateTimeUtils = dateTimeUtils;
            this.dataValidator = dataValidator;
            this.budgetSummaryItems = new ObservableCollection<BudgetSummaryItem>();

            DateTime currentDate = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            //DateTime lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            DateTime lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            this.isValidDateSelection = false;
            this.StartDate = new DateTimeOffset(firstDateOfMonth);
            this.EndDate = new DateTimeOffset(lastDateOfMonth);

        }

        [RelayCommand]
        public void DisplayBudgetSummary() {
            DateRange? monthRange = dateTimeUtils.GetMonthRange(StartDate, EndDate, IsMonthInterval);

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

        //partial void OnStartDateChanged(DateTimeOffset value) {
        //    ValidateAllProperties();
        //    OnPropertyChanged(nameof(StartDateError));
        //    OnPropertyChanged(nameof(EndDateError));
        //}

        //partial void OnEndDateChanged(DateTimeOffset value) {
        //    ValidateAllProperties();
        //    OnPropertyChanged(nameof(StartDateError));
        //    OnPropertyChanged(nameof(EndDateError));
        //}

        partial void OnStartDateChanged(DateTimeOffset value) {
            if (isMonthInterval) {
                if (isMonthInterval && dataValidator.IsValidDateSelection(StartDate, EndDate)) {
                    IsValidDateSelection = true;
                } else {
                    IsValidDateSelection = false;
                }
            } else {
                isValidDateSelection = true;
            }
        }

        partial void OnEndDateChanged(DateTimeOffset value) {
            if (isMonthInterval) {
                if (dataValidator.IsValidDateSelection(StartDate, EndDate)) {
                    IsValidDateSelection = true;
                } else {
                    IsValidDateSelection = false;
                }
            } else {
                IsValidDateSelection = true;
            }
        }

    }
}
