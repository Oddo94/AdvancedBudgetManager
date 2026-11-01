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
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManager.view_model {
    public partial class SavingsViewModel : ObservableValidator {
        [ObservableProperty]
        public DateTimeOffset startDate;

        [ObservableProperty]
        public DateTimeOffset endDate;

        [ObservableProperty]
        public DateTimeOffset monthlySavingsEvolutionDate;

        [ObservableProperty]
        public ObservableCollection<SavingDto> savingList;

        [ObservableProperty]
        public ObservableCollection<ISeries> savingPercentageFromIncomesPieSeries;

        [ObservableProperty]
        public ObservableCollection<ICartesianAxis> monthlySavingsEvolutionAxis;

        [ObservableProperty]
        public ObservableCollection<ISeries> monthlySavingsEvolutionSeries;

        [ObservableProperty]
        public bool isMonthInterval;

        [ObservableProperty]
        public bool isValidDateSelection;

        [ObservableProperty]
        public bool isEmptyGeneralSavingData;

        [ObservableProperty]
        public bool isEmptyMonthlyEvolutionData;

        [ObservableProperty]
        public string totalSavingsMessage;

        private SavingQueryService savingQueryService;

        private DateTimeUtils dateTimeUtils;

        private InputDataValidator dataValidator;

        private UIComponentInitUtils uiComponentInitUtils;

        public SavingsViewModel([NotNull] SavingQueryService savingQueryService,
        [NotNull] DateTimeUtils dateTimeUtils,
        [NotNull] InputDataValidator inputDataValidator,
        [NotNull] UIComponentInitUtils uiComponentInitUtils) {
            this.savingQueryService = savingQueryService;
            this.dateTimeUtils = dateTimeUtils;
            this.dataValidator = inputDataValidator;
            this.uiComponentInitUtils = uiComponentInitUtils;

            this.savingList = new ObservableCollection<SavingDto>();
            this.savingPercentageFromIncomesPieSeries = new ObservableCollection<ISeries>();

            List<string> defaultLabels = uiComponentInitUtils.InitColumnChartLabels(TimeUnit.Month);
            this.monthlySavingsEvolutionAxis = new ObservableCollection<ICartesianAxis>() {
               new Axis {
                   Labels = defaultLabels
               }
            };
            this.monthlySavingsEvolutionSeries = new ObservableCollection<ISeries>();

            DateTime currentDate = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            this.isValidDateSelection = false;
            this.isEmptyGeneralSavingData = false;
            this.isEmptyMonthlyEvolutionData = false;

            this.StartDate = new DateTimeOffset(firstDateOfMonth);
            this.EndDate = new DateTimeOffset(lastDateOfMonth);
            this.MonthlySavingsEvolutionDate = new DateTimeOffset(firstDateOfMonth);

            this.totalSavingsMessage = string.Empty;
        }

        [RelayCommand]
        public void DisplaySavingStatistics() {
            DateRange? monthRange = dateTimeUtils.GetMonthRange(StartDate, EndDate, IsMonthInterval);

            DisplaySavingList(monthRange);
            DisplaySavingPercentageStatistics(monthRange);

            if (SavingList.Count == 0 && SavingPercentageFromIncomesPieSeries.Count == 0) {
                this.IsEmptyGeneralSavingData = true;
            } else {
                this.IsEmptyGeneralSavingData = false;
            }
        }

        [RelayCommand]
        public void DisplayMonthlySavingsEvolution() {
            int year = MonthlySavingsEvolutionDate.Year;
            BudgetItemMonthlyEvolutionDto monthlySavingsEvolutionDto = savingQueryService.GetMonthlySavingsEvolution(year);
            Dictionary<Month, int> monthlySavingsStatistics = monthlySavingsEvolutionDto.MonthlyStatistics;

            List<string> labels = new List<string>();
            List<int> values = new List<int>();
            foreach (Month currentMonth in Enum.GetValues<Month>()) {
                if (currentMonth == Month.Undefined) {
                    continue;
                }

                int totalSavings = -1;
                monthlySavingsStatistics.TryGetValue(currentMonth, out totalSavings);

                string monthDescription = EnumExtensions.GetEnumDescription(currentMonth).Substring(0, 3);
                if (totalSavings != -1) {
                    labels.Add(monthDescription);
                    values.Add(totalSavings);
                } else {
                    labels.Add(monthDescription);
                    values.Add(0);
                }
            }

            this.IsEmptyMonthlyEvolutionData = monthlySavingsStatistics
                .ToList()
                .Select(monthRecord => monthRecord.Value > 0)
                .Count() == 0;

            MonthlySavingsEvolutionAxis.Clear();
            MonthlySavingsEvolutionAxis.Clear();

            MonthlySavingsEvolutionAxis = new ObservableCollection<ICartesianAxis>() {
                new Axis {
                    Name = "Month",
                    Labels = labels.ToArray()
                }
            };

            MonthlySavingsEvolutionSeries = new ObservableCollection<ISeries> {
                new ColumnSeries<int> {
                    Name = "Total expenses",
                    Values = values.ToArray(),
                    Fill = new SolidColorPaint(SKColors.Red)
                }
            };
        }

        private void DisplaySavingList(DateRange? monthRange) {
            if (monthRange == null) {
                return;
            }

            List<SavingDto> retrievedSavings = savingQueryService.GetSavingsByUserIdAndDateInterval(monthRange.StartDate, monthRange.EndDate);

            if (retrievedSavings.Count > 0) {
                this.SavingList.Clear();

                this.SavingList = new ObservableCollection<SavingDto>(retrievedSavings);

                this.TotalSavingsMessage = $"Displaying {retrievedSavings.Count} expenses";
            } else {
                this.SavingList = new ObservableCollection<SavingDto> { };
                this.TotalSavingsMessage = String.Empty;
            }
        }

        private void DisplaySavingPercentageStatistics(DateRange? monthRange) {
            if (monthRange == null) {
                return;
            }

            List<BudgetItemStatistics> savingsAndIncomesPercentageStatisticsList = savingQueryService.GetSavingsPercentageFromTotalIncomes(monthRange.StartDate, monthRange.EndDate);

            //List<CategoryStatisticsDto> categoriesStatisticsList = expenseCategoriesStatistics.CategoriesStatistics;
            ObservableCollection<ISeries> pieSeriesCollection = new ObservableCollection<ISeries>();

            foreach (BudgetItemStatistics itemStatistics in savingsAndIncomesPercentageStatisticsList) {
                double[] budgetItemValue = new double[] { Convert.ToDouble(itemStatistics.TotalValue) };

                if (budgetItemValue[0] > 0) {
                    pieSeriesCollection.Add(new PieSeries<double> {
                        Values = budgetItemValue,
                        Name = itemStatistics.BudgetItem.ToString(),
                        DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => {
                            double categoryValue = point.Coordinate.PrimaryValue;
                            return $"{categoryValue} ({itemStatistics.TotalPercentage})";
                        }
                    });
                }
            }

            if (pieSeriesCollection.Count > 0) {
                this.SavingPercentageFromIncomesPieSeries.Clear();
                this.SavingPercentageFromIncomesPieSeries = pieSeriesCollection;
            } else {
                this.SavingPercentageFromIncomesPieSeries = new ObservableCollection<ISeries> { };
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
