using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.exception;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.service {
    public class BudgetSummaryService {
        private IIncomeRepository incomeRepository;
        private IExpenseRepository expenseRepository;
        private IDebtRepository debtRepository;
        private ISavingRepository savingRepository;
        private IUserRepository userRepository;

        public BudgetSummaryService(IIncomeRepository incomeRepository, IExpenseRepository expenseRepository,
            IDebtRepository debtRepository, ISavingRepository savingRepository, IUserRepository userRepository) {
            this.incomeRepository = incomeRepository;
            this.expenseRepository = expenseRepository;
            this.debtRepository = debtRepository;
            this.savingRepository = savingRepository;
            this.userRepository = userRepository;
        }

        public BudgetItemStatistics GetIncomeStatistics(long userId, DateTime startDate, DateTime endDate) {
            List<Income> incomesList = incomeRepository.GetByUserIdAndDateInterval(userId, startDate, endDate);



        }

        private void ValidateInputParams(long userId, DateTime startDate, DateTime endDate) {
            User user = userRepository.GetById(userId);
            if (user == null) {
                throw new AdvancedBudgetManagerException("The supplied user ID is invalid.");
            }

            if (startDate == null || endDate == null) {
                throw new AdvancedBudgetManagerException("The start date/end date cannot be null.");
            }

            if (startDate > endDate) {
                throw new AdvancedBudgetManagerException("The start date must be prior to the end date.");
            }
        }
    }
}
