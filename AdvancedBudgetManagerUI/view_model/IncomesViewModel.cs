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
    public partial class IncomesViewModel : ObservableValidator {
        [ObservableProperty]
        public DateTimeOffset startDate;

        [ObservableProperty]
        public DateTimeOffset endDate;

        [ObservableProperty]
        public DateTimeOffset monthlyIncomeEvolutionDate;

        [ObservableProperty]
        public ObservableCollection<IncomeDto> incomeList;

        [ObservableProperty]
        public ObservableCollection<ISeries> incomeCategoriesPieSeries;

        [ObservableProperty]
        public ObservableCollection<ICartesianAxis> monthlyIncomeEvolutionAxis;

        [ObservableProperty]
        public ObservableCollection<ISeries> monthlyIncomeEvolutionSeries;

        [ObservableProperty]
        public bool isMonthInterval;

        [ObservableProperty]
        public bool isValidDateSelection;

        [ObservableProperty]
        public bool isEmptyGeneralIncomeData;

        [ObservableProperty]
        public bool isEmptyMonthlyEvolutionData;

        [ObservableProperty]
        public string totalIncomesMessage;

        public event EventHandler? NoGeneralIncomeDataFound;

        private IncomeQueryService incomeQueryService;

        private DateTimeUtils dateTimeUtils;

        private InputDataValidator dataValidator;

        private UIComponentInitUtils uiComponentInitUtils;

        public IncomesViewModel([NotNull] IncomeQueryService incomeQueryService,
            [NotNull] DateTimeUtils dateTimeUtils,
            [NotNull] InputDataValidator inputDataValidator,
            [NotNull] UIComponentInitUtils uiComponentInitUtils) {
            this.incomeQueryService = incomeQueryService;
            this.dateTimeUtils = dateTimeUtils;
            this.dataValidator = inputDataValidator;
            this.uiComponentInitUtils = uiComponentInitUtils;

            this.incomeList = new ObservableCollection<IncomeDto>();
            this.incomeCategoriesPieSeries = new ObservableCollection<ISeries>();

            List<string> defaultLabels = uiComponentInitUtils.InitColumnChartLabels(TimeUnit.Month);
            this.monthlyIncomeEvolutionAxis = new ObservableCollection<ICartesianAxis>() {
               new Axis {
                   Labels = defaultLabels
               }
            };
            this.monthlyIncomeEvolutionSeries = new ObservableCollection<ISeries>();

            DateTime currentDate = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            this.isValidDateSelection = false;
            this.isEmptyGeneralIncomeData = false;
            this.isEmptyMonthlyEvolutionData = false;

            this.StartDate = new DateTimeOffset(firstDateOfMonth);
            this.EndDate = new DateTimeOffset(lastDateOfMonth);
            this.MonthlyIncomeEvolutionDate = new DateTimeOffset(firstDateOfMonth);

            this.totalIncomesMessage = string.Empty;
        }

        [RelayCommand]
        public void DisplayIncomeStatistics() {
            DateRange? monthRange = dateTimeUtils.GetMonthRange(StartDate, EndDate, IsMonthInterval);

            DisplayIncomeList(monthRange);
            DisplayIncomeCategoryStatistics(monthRange);

            if (IncomeList.Count == 0 && IncomeCategoriesPieSeries.Count == 0) {
                this.IsEmptyGeneralIncomeData = true;
                this.NoGeneralIncomeDataFound?.Invoke(this, new CustomEventArgs("No income data was found for the specified time interval."));
            } else {
                this.IsEmptyGeneralIncomeData = false;
            }
        }

        [RelayCommand]
        public void DisplayMonthlyIncomeEvolution() {
            int year = MonthlyIncomeEvolutionDate.Year;
            BudgetItemMonthlyEvolutionDto monthlyIncomeEvolutionDto = incomeQueryService.GetMonthlyIncomeEvolution(year);
            Dictionary<Month, int> monthlyIncomeStatistics = monthlyIncomeEvolutionDto.MonthlyStatistics;

            List<string> labels = new List<string>();
            List<int> values = new List<int>();
            foreach (Month currentMonth in Enum.GetValues<Month>()) {
                if (currentMonth == Month.Undefined) {
                    continue;
                }

                int totalIncomes = -1;
                monthlyIncomeStatistics.TryGetValue(currentMonth, out totalIncomes);

                string monthDescription = EnumExtensions.GetEnumDescription(currentMonth).Substring(0, 3);
                if (totalIncomes != -1) {
                    labels.Add(monthDescription);
                    values.Add(totalIncomes);
                } else {
                    labels.Add(monthDescription);
                    values.Add(0);
                }
            }

            this.IsEmptyMonthlyEvolutionData = monthlyIncomeStatistics
                .ToList()
                .Select(monthRecord => monthRecord.Value > 0)
                .Count() == 0;

            //if (hasMonthlyEvolutionData) {
            MonthlyIncomeEvolutionAxis.Clear();
            MonthlyIncomeEvolutionSeries.Clear();

            MonthlyIncomeEvolutionAxis = new ObservableCollection<ICartesianAxis>() {
                new Axis {
                    Name = "Month",
                    Labels = labels.ToArray()
                }
            };

            MonthlyIncomeEvolutionSeries = new ObservableCollection<ISeries> {
                new ColumnSeries<int> {
                    Name = "Total incomes",
                    Values = values.ToArray(),
                    Fill = new SolidColorPaint(SKColors.DodgerBlue)
                }
            };
        }

        private void DisplayIncomeList(DateRange? monthRange) {
            if (monthRange == null) {
                return;
            }

            List<IncomeDto> retrievedIncomes = incomeQueryService.GetIncomesByUserIdAndDateInterval(monthRange.StartDate, monthRange.EndDate);

            if (retrievedIncomes.Count > 0) {
                this.IncomeList.Clear();

                this.IncomeList = new ObservableCollection<IncomeDto>(retrievedIncomes);

                this.TotalIncomesMessage = $"Displaying {retrievedIncomes.Count} incomes";
            } else {
                this.IncomeList = new ObservableCollection<IncomeDto> { };
            }
        }

        private void DisplayIncomeCategoryStatistics(DateRange? monthRange) {
            if (monthRange == null) {
                return;
            }

            BudgetItemCategoriesStatisticsDto incomeCategoriesStatistics = incomeQueryService.GetAggregatedIncomesByCategory(monthRange.StartDate, monthRange.EndDate);

            List<CategoryStatisticsDto> categoriesStatisticsList = incomeCategoriesStatistics.CategoriesStatistics;
            ObservableCollection<ISeries> pieSeriesCollection = new ObservableCollection<ISeries>();

            foreach (CategoryStatisticsDto incomeCategory in categoriesStatisticsList) {
                double[] incomeCategoryValue = new double[] { Convert.ToDouble(incomeCategory.Value) };

                if (incomeCategoryValue[0] > 0) {
                    pieSeriesCollection.Add(new PieSeries<double> {
                        Values = incomeCategoryValue,
                        Name = incomeCategory.Name,
                        DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => {
                            double categoryValue = point.Coordinate.PrimaryValue;
                            return $"{categoryValue} ({incomeCategory.Percentage})";
                        }
                    });
                }
            }

            if (pieSeriesCollection.Count > 0) {
                this.IncomeCategoriesPieSeries.Clear();
                this.IncomeCategoriesPieSeries = pieSeriesCollection;
            } else {
                this.IncomeCategoriesPieSeries = new ObservableCollection<ISeries> { };
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
