using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.service;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
    public class BudgetSummaryViewModel {
        private DateTime budgetSummaryStartDate;

        private DateTime budgetSummaryEndDate;

        private BudgetSummaryService budgetSummaryService;

        public BudgetSummaryViewModel([NotNull] BudgetSummaryService budgetSummaryService) {
            this.budgetSummaryService = budgetSummaryService;
        }

        public void DisplayBudgetSummary() {
            BudgetSummaryDto budgetSummaryDto = budgetSummaryService.GetBudgetSummaryInfo(budgetSummaryStartDate, budgetSummaryEndDate);
        }
    }
}
