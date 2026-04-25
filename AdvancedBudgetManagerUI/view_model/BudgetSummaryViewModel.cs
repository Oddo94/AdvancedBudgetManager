using AdvancedBudgetManager.utils;
using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class BudgetSummaryViewModel : ObservableValidator {
        [ObservableProperty]
        public DateTimeOffset startDate;

        [ObservableProperty]
        public DateTimeOffset endDate;

        [ObservableProperty]
        public DateTimeOffset dailyExpenseTotalsDate;

        [ObservableProperty]
        public bool isMonthInterval;

        [ObservableProperty]
        private ObservableCollection<BudgetSummaryItem> budgetSummaryItems;

        [ObservableProperty]
        private ObservableCollection<ISeries> pieSeries;

        [ObservableProperty]
        private ObservableCollection<ICartesianAxis> dailyExpenseTotalAxis;

        [ObservableProperty]
        private ObservableCollection<ISeries> dailyExpenseTotalSeries;

        [ObservableProperty]
        public bool isValidDateSelection;

        private DateTime normalizedStartDate;

        private DateTime normalizedEndDate;

        private BudgetSummaryService budgetSummaryService;

        private DateTimeUtils dateTimeUtils;

        private InputDataValidator dataValidator;

        private UIComponentInitUtils uiComponentInitUtils;


        public BudgetSummaryViewModel([NotNull] BudgetSummaryService budgetSummaryService,
            [NotNull] DateTimeUtils dateTimeUtils,
            [NotNull] InputDataValidator dataValidator,
            [NotNull] UIComponentInitUtils uiComponentInitUtils) {
            this.budgetSummaryService = budgetSummaryService;
            this.dateTimeUtils = dateTimeUtils;
            this.dataValidator = dataValidator;
            this.uiComponentInitUtils = uiComponentInitUtils;
            this.budgetSummaryItems = new ObservableCollection<BudgetSummaryItem>();
            this.pieSeries = new ObservableCollection<ISeries>();

            List<string> defaultLabels = uiComponentInitUtils.InitColumnChartLabels(TimeUnit.Day);

            this.dailyExpenseTotalAxis = new ObservableCollection<ICartesianAxis>() {
                new Axis {
                    Labels = defaultLabels
                }
            };

            this.dailyExpenseTotalSeries = new ObservableCollection<ISeries>();

            DateTime currentDate = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            this.isValidDateSelection = false;
            this.StartDate = new DateTimeOffset(firstDateOfMonth);
            this.EndDate = new DateTimeOffset(lastDateOfMonth);
            this.DailyExpenseTotalsDate = new DateTimeOffset(firstDateOfMonth);

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

            UpdateBudgetSummaryPieChart(budgetSummaryDto);
        }

        [RelayCommand]
        public void DisplayDailyExpenseTotals() {
            DateTime dailyExpenseTotalsStartDate = DailyExpenseTotalsDate.Date;
            DateTime dailyExpenseTotalsEndDate = dailyExpenseTotalsStartDate.AddMonths(1).AddDays(-1);
            int totalDaysForSelectedMonth = DateTime.DaysInMonth(dailyExpenseTotalsStartDate.Year, dailyExpenseTotalsStartDate.Month);

            Dictionary<int, double> dailyExpenseTotals = budgetSummaryService.GetDailyExpenseTotals(dailyExpenseTotalsStartDate, dailyExpenseTotalsEndDate);

            List<int> labels = new List<int>();
            List<double> values = new List<double>();
            for (int i = 1; i <= totalDaysForSelectedMonth; i++) {
                double currentDailyExpenseTotal = -1;
                dailyExpenseTotals.TryGetValue(i, out currentDailyExpenseTotal);

                //Check to see if it can be simplified (set currentDailyExpenseTotal to 0)
                if (currentDailyExpenseTotal != -1) {
                    labels.Add(i);
                    values.Add(currentDailyExpenseTotal);
                } else {
                    labels.Add(i);
                    values.Add(0);
                }
            }

            DailyExpenseTotalAxis.Clear();
            DailyExpenseTotalSeries.Clear();

            DailyExpenseTotalAxis = new ObservableCollection<ICartesianAxis>() { new Axis {
                Name = "Day of month",
                Labels = labels
                    .ToList()
                    .Select(x => Convert.ToString(x))
                    .ToArray()
                }
            };

            DailyExpenseTotalSeries = new ObservableCollection<ISeries> {
                new ColumnSeries<double> {
                    Name = "Total expenses",
                    Values = values.ToArray(),
                    Fill = new SolidColorPaint(SKColors.Gold)
                }
            }; ;

        }

        private void UpdateBudgetSummaryPieChart(BudgetSummaryDto budgetSummaryDto) {
            if (budgetSummaryDto == null) {
                return;
            }

            BudgetSummaryItem expensesItem = new BudgetSummaryItem("Expenses", budgetSummaryDto.TotalExpenses, budgetSummaryDto.TotalExpensesPercentage);
            BudgetSummaryItem debtsItem = new BudgetSummaryItem("Debts", budgetSummaryDto.TotalDebts, budgetSummaryDto.TotalDebtsPercentage);
            BudgetSummaryItem savingsItem = new BudgetSummaryItem("Savings", budgetSummaryDto.TotalSavings, budgetSummaryDto.TotalSavingsPercentage);
            BudgetSummaryItem leftToSpendItem = new BudgetSummaryItem("Left to spend", budgetSummaryDto.TotalLeftToSpend, budgetSummaryDto.TotalLeftToSpendPercentage);

            List<BudgetSummaryItem> budgetSummaryItems = new List<BudgetSummaryItem>() { expensesItem, debtsItem, savingsItem, leftToSpendItem };
            ObservableCollection<ISeries> pieSeriesCollection = new ObservableCollection<ISeries>();

            foreach (BudgetSummaryItem item in budgetSummaryItems) {
                double[] budgetSummaryItemValue = new double[] { Convert.ToDouble(item.TotalValue) };

                //The elements whose value is equal to 0 will not be shown because they are irrelevant
                if (budgetSummaryItemValue[0] > 0) {
                    pieSeriesCollection.Add(new PieSeries<double> {
                        Values = budgetSummaryItemValue, Name = item.ItemName,
                        DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => {
                            double itemValue = point.Coordinate.PrimaryValue;
                            return $"{itemValue} ({item.TotalPercentage}%)";
                        }
                    });
                }
            }

            this.PieSeries = pieSeriesCollection;
        }

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
