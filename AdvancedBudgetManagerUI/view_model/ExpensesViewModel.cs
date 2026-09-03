using AdvancedBudgetManager.utils;
using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.service.query;
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

namespace AdvancedBudgetManager.view_model {
    public partial class ExpensesViewModel : ObservableValidator {
        [ObservableProperty]
        public DateTimeOffset startDate;

        [ObservableProperty]
        public DateTimeOffset endDate;

        [ObservableProperty]
        public DateTimeOffset monthlyExpensesEvolutionDate;

        [ObservableProperty]
        public ObservableCollection<ExpenseDto> expenseList;

        [ObservableProperty]
        public ObservableCollection<ISeries> expenseCategoriesPieSeries;

        [ObservableProperty]
        public ObservableCollection<ICartesianAxis> monthlyExpensesEvolutionAxis;

        [ObservableProperty]
        public ObservableCollection<ISeries> monthlyExpensesEvolutionSeries;

        [ObservableProperty]
        public bool isMonthInterval;

        [ObservableProperty]
        public bool isValidDateSelection;

        [ObservableProperty]
        public bool isEmptyGeneralExpenseData;

        [ObservableProperty]
        public bool isEmptyMonthlyEvolutionData;

        [ObservableProperty]
        public string totalExpensesMessage;

        //public event EventHandler? NoGeneralExpenseDataFound;

        private ExpenseQueryService expenseQueryService;

        private DateTimeUtils dateTimeUtils;

        private InputDataValidator dataValidator;

        private UIComponentInitUtils uiComponentInitUtils;

        public ExpensesViewModel([NotNull] ExpenseQueryService expenseQueryService,
        [NotNull] DateTimeUtils dateTimeUtils,
        [NotNull] InputDataValidator inputDataValidator,
        [NotNull] UIComponentInitUtils uiComponentInitUtils) {
            this.expenseQueryService = expenseQueryService;
            this.dateTimeUtils = dateTimeUtils;
            this.dataValidator = inputDataValidator;
            this.uiComponentInitUtils = uiComponentInitUtils;

            this.expenseList = new ObservableCollection<ExpenseDto>();
            this.expenseCategoriesPieSeries = new ObservableCollection<ISeries>();

            List<string> defaultLabels = uiComponentInitUtils.InitColumnChartLabels(TimeUnit.Month);
            this.monthlyExpensesEvolutionAxis = new ObservableCollection<ICartesianAxis>() {
               new Axis {
                   Labels = defaultLabels
               }
            };
            this.monthlyExpensesEvolutionSeries = new ObservableCollection<ISeries>();

            DateTime currentDate = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            this.isValidDateSelection = false;
            this.isEmptyGeneralExpenseData = false;
            this.isEmptyMonthlyEvolutionData = false;

            this.StartDate = new DateTimeOffset(firstDateOfMonth);
            this.EndDate = new DateTimeOffset(lastDateOfMonth);
            this.MonthlyExpensesEvolutionDate = new DateTimeOffset(firstDateOfMonth);

            this.totalExpensesMessage = string.Empty;
        }

        [RelayCommand]
        public void DisplayExpenseStatistics() {
            DateRange? monthRange = dateTimeUtils.GetMonthRange(StartDate, EndDate, IsMonthInterval);

            DisplayExpenseList(monthRange);
            DisplayExpenseCategoryStatistics(monthRange);

            if (ExpenseList.Count == 0 && ExpenseCategoriesPieSeries.Count == 0) {
                this.IsEmptyGeneralExpenseData = true;
                //this.NoGeneralExpenseDataFound?.Invoke(this, new CustomEventArgs("No income data was found for the specified time interval."));
            } else {
                this.IsEmptyGeneralExpenseData = false;
            }
        }

        [RelayCommand]
        public void DisplayMonthlyExpensesEvolution() {
            int year = MonthlyExpensesEvolutionDate.Year;
            BudgetItemMonthlyEvolutionDto monthlyExpensesEvolutionDto = expenseQueryService.GetMonthlyExpensesEvolution(year);
            Dictionary<Month, int> monthlyExpensesStatistics = monthlyExpensesEvolutionDto.MonthlyStatistics;

            List<string> labels = new List<string>();
            List<int> values = new List<int>();
            foreach (Month currentMonth in Enum.GetValues<Month>()) {
                if (currentMonth == Month.Undefined) {
                    continue;
                }

                int totalExpenses = -1;
                monthlyExpensesStatistics.TryGetValue(currentMonth, out totalExpenses);

                string monthDescription = EnumExtensions.GetEnumDescription(currentMonth).Substring(0, 3);
                if (totalExpenses != -1) {
                    labels.Add(monthDescription);
                    values.Add(totalExpenses);
                } else {
                    labels.Add(monthDescription);
                    values.Add(0);
                }
            }

            this.IsEmptyMonthlyEvolutionData = monthlyExpensesStatistics
                .ToList()
                .Select(monthRecord => monthRecord.Value > 0)
                .Count() == 0;

            //if (hasMonthlyEvolutionData) {
            MonthlyExpensesEvolutionAxis.Clear();
            MonthlyExpensesEvolutionSeries.Clear();

            MonthlyExpensesEvolutionAxis = new ObservableCollection<ICartesianAxis>() {
                new Axis {
                    Name = "Month",
                    Labels = labels.ToArray()
                }
            };

            MonthlyExpensesEvolutionSeries = new ObservableCollection<ISeries> {
                new ColumnSeries<int> {
                    Name = "Total expenses",
                    Values = values.ToArray(),
                    Fill = new SolidColorPaint(SKColors.Red)
                }
            };
        }

        private void DisplayExpenseList(DateRange? monthRange) {
            if (monthRange == null) {
                return;
            }

            List<ExpenseDto> retrievedExpenses = expenseQueryService.GetExpensesByUserIdAndDateInterval(monthRange.StartDate, monthRange.EndDate);

            if (retrievedExpenses.Count > 0) {
                this.ExpenseList.Clear();

                this.ExpenseList = new ObservableCollection<ExpenseDto>(retrievedExpenses);

                this.TotalExpensesMessage = $"Displaying {retrievedExpenses.Count} expenses";
            } else {
                this.ExpenseList = new ObservableCollection<ExpenseDto> { };
                this.TotalExpensesMessage = String.Empty;
            }
        }

        private void DisplayExpenseCategoryStatistics(DateRange? monthRange) {
            if (monthRange == null) {
                return;
            }

            BudgetItemCategoriesStatisticsDto expenseCategoriesStatistics = expenseQueryService.GetAggregatedExpensesByCategory(monthRange.StartDate, monthRange.EndDate);

            List<CategoryStatisticsDto> categoriesStatisticsList = expenseCategoriesStatistics.CategoriesStatistics;
            ObservableCollection<ISeries> pieSeriesCollection = new ObservableCollection<ISeries>();

            foreach (CategoryStatisticsDto expenseCategory in categoriesStatisticsList) {
                double[] expenseCategoryValue = new double[] { Convert.ToDouble(expenseCategory.Value) };

                if (expenseCategoryValue[0] > 0) {
                    pieSeriesCollection.Add(new PieSeries<double> {
                        Values = expenseCategoryValue,
                        Name = expenseCategory.Name,
                        DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => {
                            double categoryValue = point.Coordinate.PrimaryValue;
                            return $"{categoryValue} ({expenseCategory.Percentage})";
                        }
                    });
                }
            }

            if (pieSeriesCollection.Count > 0) {
                this.ExpenseCategoriesPieSeries.Clear();
                this.ExpenseCategoriesPieSeries = pieSeriesCollection;
            } else {
                this.ExpenseCategoriesPieSeries = new ObservableCollection<ISeries> { };
            }
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
