using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.exception;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedBudgetManagerCore.service {
    public class BudgetSummaryService {
        private IIncomeRepository incomeRepository;
        private IExpenseRepository expenseRepository;
        private IDebtRepository debtRepository;
        private ISavingRepository savingRepository;
        private IUserRepository userRepository;

        private IUserSessionService userSessionService;

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

        public BudgetSummaryDto GetBudgetSummaryInfo(DateTime startDate, DateTime endDate) {
            long userId = userSessionService.AuthenticatedUser.UserId;
            BudgetItemStatistics incomeStatistics = GetIncomeStatistics(userId, startDate, endDate);
            BudgetItemStatistics expenseStatistics = GetExpenseStatistics(userId, startDate, endDate);
            BudgetItemStatistics debtStatistics = GetDebtStatistics(userId, startDate, endDate);
            BudgetItemStatistics savingStatistics = GetSavingStatistics(userId, startDate, endDate);

            BudgetSummaryDto budgetSummaryDto = new BudgetSummaryDto(
                incomeStatistics.TotalValue,
                incomeStatistics.TotalPercentage,
                expenseStatistics.TotalValue,
                expenseStatistics.TotalPercentage,
                debtStatistics.TotalValue,
                debtStatistics.TotalPercentage,
                savingStatistics.TotalValue,
                savingStatistics.TotalPercentage);

            return budgetSummaryDto;
        }
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

        public BudgetItemStatistics GetExpenseStatistics(long userId, DateTime startDate, DateTime endDate) {
            ValidateInputParams(userId, startDate, endDate);

            int expenseSum = expenseRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(expense => expense.Value)
                .DefaultIfEmpty(0)
                .Sum();

            int totalIncomes = incomeRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(income => income.Value)
                .DefaultIfEmpty(0)
                .Sum();

            double totalPercentage = expenseSum * 100 / totalIncomes;
            BudgetItemStatistics expenseStatistics = new BudgetItemStatistics(expenseSum, totalPercentage);

            return expenseStatistics;
        }

        public BudgetItemStatistics GetDebtStatistics(long userId, DateTime startDate, DateTime endDate) {
            ValidateInputParams(userId, startDate, endDate);

            int debtSum = debtRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(debt => debt.Value)
                .DefaultIfEmpty(0)
                .Sum();

            int totalIncomes = incomeRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(income => income.Value)
                .DefaultIfEmpty(0)
                .Sum();

            double totalPercentage = debtSum * 100 / totalIncomes;
            BudgetItemStatistics debtStatistics = new BudgetItemStatistics(debtSum, totalPercentage);

            return debtStatistics;
        }

        public BudgetItemStatistics GetSavingStatistics(long userId, DateTime startDate, DateTime endDate) {
            ValidateInputParams(userId, startDate, endDate);

            int savingSum = savingRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(saving => saving.Value)
                .DefaultIfEmpty(0)
                .Sum();

            int totalIncomes = incomeRepository.GetByUserIdAndDateInterval(userId, startDate, endDate)
                .Select(income => income.Value)
                .DefaultIfEmpty(0)
                .Sum();

            double totalPercentage = savingSum * 100 / totalIncomes;
            BudgetItemStatistics savingStatistics = new BudgetItemStatistics(savingSum, totalPercentage);

            return savingStatistics;
        }

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
