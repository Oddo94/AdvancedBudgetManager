using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.exception;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedBudgetManagerCore.service {
    /// <summary>
    /// Service class used for providing the aggregated data related to the budget summary.
    /// </summary>
    public class BudgetSummaryService {
        /// <summary>
        /// The income repository.
        /// </summary>
        private IIncomeRepository incomeRepository;

        /// <summary>
        /// The expense repository.
        /// </summary>
        private IExpenseRepository expenseRepository;

        /// <summary>
        /// The debt repository.
        /// </summary>
        private IDebtRepository debtRepository;

        /// <summary>
        /// The saving repository.
        /// </summary>
        private ISavingRepository savingRepository;

        /// <summary>
        /// The user repository.
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// The user session service.
        /// </summary>
        private IUserSessionService userSessionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetSummaryService"/> based on the provided parameters.
        /// </summary>
        /// <param name="incomeRepository">The income repository.</param>
        /// <param name="expenseRepository">The expense repository.</param>
        /// <param name="debtRepository">The debt repository.</param>
        /// <param name="savingRepository">The saving repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="userSessionService">The user session service.</param>
        public BudgetSummaryService(IIncomeRepository incomeRepository,
            IExpenseRepository expenseRepository,
            IDebtRepository debtRepository,
            ISavingRepository savingRepository,
            IUserRepository userRepository,
            IUserSessionService userSessionService) {
            this.incomeRepository = incomeRepository;
            this.expenseRepository = expenseRepository;
            this.debtRepository = debtRepository;
            this.savingRepository = savingRepository;
            this.userRepository = userRepository;
            this.userSessionService = userSessionService;
        }

        /// <summary>
        /// Retrieves the budget summary information for a specified date interval.
        /// </summary>
        /// <param name="startDate">The start date of the interval.</param>
        /// <param name="endDate">The end date of the interval.</param>
        /// <returns>A <see cref="BudgetSummaryDto"/> object containing the budget summary information.</returns>
        public BudgetSummaryDto GetBudgetSummaryInfo(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;
            BudgetItemStatistics incomeStatistics = GetIncomeStatistics(userId, startDate, endDate);
            BudgetItemStatistics expenseStatistics = GetExpenseStatistics(userId, startDate, endDate, incomeStatistics.TotalValue);
            BudgetItemStatistics debtStatistics = GetDebtStatistics(userId, startDate, endDate, incomeStatistics.TotalValue);
            BudgetItemStatistics savingStatistics = GetSavingStatistics(userId, startDate, endDate, incomeStatistics.TotalValue);
            BudgetItemStatistics totalLeftToSpendStatistics = GetTotalLeftToSpendStatistics(incomeStatistics.TotalValue, expenseStatistics.TotalValue, debtStatistics.TotalValue, savingStatistics.TotalValue);

            BudgetSummaryDto budgetSummaryDto = new BudgetSummaryDto(
                incomeStatistics.TotalValue,
                incomeStatistics.TotalPercentage,
                expenseStatistics.TotalValue,
                expenseStatistics.TotalPercentage,
                debtStatistics.TotalValue,
                debtStatistics.TotalPercentage,
                savingStatistics.TotalValue,
                savingStatistics.TotalPercentage,
                totalLeftToSpendStatistics.TotalValue,
                totalLeftToSpendStatistics.TotalPercentage);

            return budgetSummaryDto;
        }

        /// <summary>
        /// Retrieves the daily expense totals for a specified date interval.
        /// </summary>
        /// <param name="startDate">The start date of the interval.</param>
        /// <param name="endDate">The end date of the interval.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> object whose keys represent the days of the month and whose values represent the daily total expenses.</returns>
        public Dictionary<int, double> GetDailyExpenseTotals(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;
            ValidateInputParams(userId, startDate, endDate);

            List<DailyExpenseTotalDto> dailyTotals = expenseRepository.GetDailyExpenseTotalsForDateInterval(userId, startDate, endDate);

            Dictionary<int, double> dailyExpenseTotals = new Dictionary<int, double>();
            foreach (DailyExpenseTotalDto dailyTotal in dailyTotals) {
                dailyExpenseTotals.Add(dailyTotal.Day, dailyTotal.TotalValue);
            }

            return dailyExpenseTotals;
        }

        /// <summary>
        /// Calculates the income statistics for a specified date interval, based on a user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="startDate">The start date of the interval.</param>
        /// <param name="endDate">The end date of the interval.</param>
        /// <returns>A <see cref="BudgetItemStatistics"/> object containing the income statistics.</returns>
        public BudgetItemStatistics GetIncomeStatistics(long userId, DateTime startDate, DateTime endDate) {
            ValidateInputParams(userId, startDate, endDate);

            List<Income> incomesList = incomeRepository.GetByUserIdAndDateInterval(userId, startDate, endDate);
            BudgetItemStatistics incomeStatistics;

            if (incomesList.Count > 0) {
                int incomeSum = incomesList.Sum(income => income.Value);
                double totalPercentage = 100;
                incomeStatistics = new BudgetItemStatistics(incomeSum, totalPercentage);
            } else {
                int incomeSum = 0;
                double totalPercentage = 100;
                incomeStatistics = new BudgetItemStatistics(incomeSum, totalPercentage);
            }

            return incomeStatistics;
        }

        /// <summary>
        /// Calculates the expense statistics for a specified date interval, based on a user id and total incomes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="startDate">The start date of the interval.</param>
        /// <param name="endDate">The end date of the interval.</param>
        /// <param name="totalIncomes">The total incomes.</param>
        /// <returns>A <see cref="BudgetItemStatistics"/> object containing the expense statistics.</returns>
        public BudgetItemStatistics GetExpenseStatistics(long userId, DateTime startDate, DateTime endDate, int totalIncomes) {
            ValidateInputParams(userId, startDate, endDate);

            int expenseSum = expenseRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(expense => expense.Value)
                .DefaultIfEmpty(0)
                .Sum();

            double totalPercentage = totalIncomes > 0 ? expenseSum * 100 / totalIncomes : 0;
            BudgetItemStatistics expenseStatistics = new BudgetItemStatistics(expenseSum, totalPercentage);

            return expenseStatistics;
        }

        /// <summary>
        /// Calculates the debt statistics for a specified date interval, based on a user id and total incomes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="startDate">The start date of the interval.</param>
        /// <param name="endDate">The end date of the interval.</param>
        /// <param name="totalIncomes">The total incomes.</param>
        /// <returns>A <see cref="BudgetItemStatistics"/> object containing the debt statistics.</returns>
        public BudgetItemStatistics GetDebtStatistics(long userId, DateTime startDate, DateTime endDate, int totalIncomes) {
            ValidateInputParams(userId, startDate, endDate);

            int debtSum = debtRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(debt => debt.Value)
                .DefaultIfEmpty(0)
                .Sum();

            double totalPercentage = totalIncomes > 0 ? debtSum * 100 / totalIncomes : 0;
            BudgetItemStatistics debtStatistics = new BudgetItemStatistics(debtSum, totalPercentage);

            return debtStatistics;
        }

        /// <summary>
        /// Calculates the saving statistics for a specified date interval, based on a user id and total incomes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="startDate">The start date of the interval.</param>
        /// <param name="endDate">The end date of the interval.</param>
        /// <param name="totalIncomes">The total incomes.</param>
        /// <returns>A <see cref="BudgetItemStatistics"/> object containing the saving statistics.</returns>
        public BudgetItemStatistics GetSavingStatistics(long userId, DateTime startDate, DateTime endDate, int totalIncomes) {
            ValidateInputParams(userId, startDate, endDate);

            int savingSum = savingRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(saving => saving.Value)
                .DefaultIfEmpty(0)
                .Sum();

            double totalPercentage = totalIncomes > 0 ? savingSum * 100 / totalIncomes : 0;
            BudgetItemStatistics savingStatistics = new BudgetItemStatistics(savingSum, totalPercentage);

            return savingStatistics;
        }

        /// <summary>
        /// Calculates the total left to spend statistics based on the provided parameters.
        /// </summary>
        /// <param name="totalIncomes">The total incomes.</param>
        /// <param name="totalExpenses">The total expenses.</param>
        /// <param name="totalDebts">The total debst.</param>
        /// <param name="totalSavings">The total savings.</param>
        /// <returns>A <see cref="BudgetItemStatistics"/> object containing the total left to spend statistics.</returns>
        public BudgetItemStatistics GetTotalLeftToSpendStatistics(int totalIncomes, int totalExpenses, int totalDebts, int totalSavings) {
            if (totalIncomes <= 0) {
                return new BudgetItemStatistics(0, 0);
            }

            int totalLeftToSpend = totalIncomes - (totalExpenses + totalDebts + totalSavings);
            double totalLeftToSpendPercentage = Math.Round(totalLeftToSpend * 100 / (double)totalIncomes, 2);

            return new BudgetItemStatistics(totalLeftToSpend, totalLeftToSpendPercentage);
        }

        /// <summary>
        /// Validates the provided input parameters.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <exception cref="AdvancedBudgetManagerException"></exception>
        private void ValidateInputParams(long userId, DateTime startDate, DateTime endDate) {
            User user = userRepository.GetById(userId);
            if (user == null) {
                throw new AdvancedBudgetManagerException("The supplied user ID is invalid.");
            }

            if (startDate > endDate) {
                throw new AdvancedBudgetManagerException("The start date must be prior to the end date.");
            }
        }
    }
}
