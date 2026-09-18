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
    public partial class DebtsViewModel : ObservableValidator {
        [ObservableProperty]
        public DateTimeOffset startDate;

        [ObservableProperty]
        public DateTimeOffset endDate;

        [ObservableProperty]
        public DateTimeOffset monthlyDebtsEvolutionDate;

        [ObservableProperty]
        public ObservableCollection<DebtDto> debtList;

        [ObservableProperty]
        public ObservableCollection<ISeries> creditorStatisticsPieSeries;

        [ObservableProperty]
        public ObservableCollection<ICartesianAxis> monthlyDebtsEvolutionAxis;

        [ObservableProperty]
        public ObservableCollection<ISeries> monthlyDebtsEvolutionSeries;

        [ObservableProperty]
        public bool isMonthInterval;

        [ObservableProperty]
        public bool isValidDateSelection;

        [ObservableProperty]
        public bool isEmptyGeneralDebtData;

        [ObservableProperty]
        public bool isEmptyMonthlyEvolutionData;

        [ObservableProperty]
        public string totalDebtsMessage;

        private DebtsQueryService debtsQueryService;

        private DateTimeUtils dateTimeUtils;

        private InputDataValidator dataValidator;

        private UIComponentInitUtils uiComponentInitUtils;

        public DebtsViewModel([NotNull] DebtsQueryService debtsQueryService,
  [NotNull] DateTimeUtils dateTimeUtils,
  [NotNull] InputDataValidator inputDataValidator,
  [NotNull] UIComponentInitUtils uiComponentInitUtils) {
            this.debtsQueryService = debtsQueryService;
            this.dateTimeUtils = dateTimeUtils;
            this.dataValidator = inputDataValidator;
            this.uiComponentInitUtils = uiComponentInitUtils;

            this.debtList = new ObservableCollection<DebtDto>();
            this.creditorStatisticsPieSeries = new ObservableCollection<ISeries>();

            List<string> defaultLabels = uiComponentInitUtils.InitColumnChartLabels(TimeUnit.Month);
            this.monthlyDebtsEvolutionAxis = new ObservableCollection<ICartesianAxis>() {
               new Axis {
                   Labels = defaultLabels
               }
            };
            this.monthlyDebtsEvolutionSeries = new ObservableCollection<ISeries>();

            DateTime currentDate = DateTime.Now;
            DateTime firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            this.isValidDateSelection = false;
            this.isEmptyGeneralDebtData = false;
            this.isEmptyMonthlyEvolutionData = false;

            this.StartDate = new DateTimeOffset(firstDateOfMonth);
            this.EndDate = new DateTimeOffset(lastDateOfMonth);
            this.MonthlyDebtsEvolutionDate = new DateTimeOffset(firstDateOfMonth);

            this.totalDebtsMessage = string.Empty;
        }

        [RelayCommand]
        public void DisplayDebtCreditorStatistics() {
            DateRange? monthRange = dateTimeUtils.GetMonthRange(StartDate, EndDate, IsMonthInterval);

            DisplayDebtList(monthRange);
            DisplayDebtCreditorStatistics(monthRange);

            if (DebtList.Count == 0 && CreditorStatisticsPieSeries.Count == 0) {
                this.IsEmptyGeneralDebtData = true;
            } else {
                this.IsEmptyGeneralDebtData = false;
            }
        }

        [RelayCommand]
        public void DisplayMonthlyDebtsEvolution() {
            int year = MonthlyDebtsEvolutionDate.Year;
            BudgetItemMonthlyEvolutionDto monthlyDebtsEvolutionDto = debtsQueryService.GetMonthlyDebtsEvolution(year);
            Dictionary<Month, int> monthlyDebtsStatistics = monthlyDebtsEvolutionDto.MonthlyStatistics;

            List<string> labels = new List<string>();
            List<int> values = new List<int>();
            foreach (Month currentMonth in Enum.GetValues<Month>()) {
                if (currentMonth == Month.Undefined) {
                    continue;
                }

                int totalDebts = -1;
                monthlyDebtsStatistics.TryGetValue(currentMonth, out totalDebts);

                string monthDescription = EnumExtensions.GetEnumDescription(currentMonth).Substring(0, 3);
                if (totalDebts != -1) {
                    labels.Add(monthDescription);
                    values.Add(totalDebts);
                } else {
                    labels.Add(monthDescription);
                    values.Add(0);
                }
            }

            this.IsEmptyMonthlyEvolutionData = monthlyDebtsStatistics
                .ToList()
                .Select(monthRecord => monthRecord.Value > 0)
                .Count() == 0;

            MonthlyDebtsEvolutionAxis.Clear();
            MonthlyDebtsEvolutionSeries.Clear();

            MonthlyDebtsEvolutionAxis = new ObservableCollection<ICartesianAxis>() {
                new Axis {
                    Name = "Month",
                    Labels = labels.ToArray()
                }
            };

            MonthlyDebtsEvolutionSeries = new ObservableCollection<ISeries> {
                new ColumnSeries<int> {
                    Name = "Total debts",
                    Values = values.ToArray(),
                    Fill = new SolidColorPaint(SKColors.MediumPurple)
                }
            };
        }

        private void DisplayDebtList(DateRange? monthRange) {
            if (monthRange == null) {
                return;
            }

            List<DebtDto> retrievedDebts = debtsQueryService.GetDebtsByUserIdAndDateInterval(monthRange.StartDate, monthRange.EndDate);

            if (retrievedDebts.Count > 0) {
                this.DebtList.Clear();

                this.DebtList = new ObservableCollection<DebtDto>(retrievedDebts);

                this.TotalDebtsMessage = $"Displaying {retrievedDebts.Count} debts";
            } else {
                this.DebtList = new ObservableCollection<DebtDto> { };
                this.TotalDebtsMessage = String.Empty;
            }
        }

        private void DisplayDebtCreditorStatistics(DateRange? monthRange) {
            if (monthRange == null) {
                return;
            }

            BudgetItemCategoriesStatisticsDto debtCreditorStatistics = debtsQueryService.GetAggregatedDebtsByCreditor(monthRange.StartDate, monthRange.EndDate);

            List<CategoryStatisticsDto> debtCreditorsList = debtCreditorStatistics.CategoriesStatistics;
            ObservableCollection<ISeries> pieSeriesCollection = new ObservableCollection<ISeries>();

            foreach (CategoryStatisticsDto debtCreditorStatistic in debtCreditorsList) {
                double[] totalDebtValueForCreditor = new double[] { Convert.ToDouble(debtCreditorStatistic.Value) };

                if (totalDebtValueForCreditor[0] > 0) {
                    pieSeriesCollection.Add(new PieSeries<double> {
                        Values = totalDebtValueForCreditor,
                        Name = debtCreditorStatistic.Name,
                        DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => {
                            double categoryValue = point.Coordinate.PrimaryValue;
                            return $"{categoryValue} ({debtCreditorStatistic.Percentage})";
                        }
                    });
                }
            }

            if (pieSeriesCollection.Count > 0) {
                this.CreditorStatisticsPieSeries.Clear();
                this.CreditorStatisticsPieSeries = pieSeriesCollection;
            } else {
                this.CreditorStatisticsPieSeries = new ObservableCollection<ISeries> { };
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
