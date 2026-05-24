using AdvancedBudgetManagerCore.utils.enums;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.model.dto {
    public class BudgetItemMonthlyEvolutionDto {
        BudgetItem budgetItem;
        //List<MonthlyStatisticsDto> monthlyStatistics;
        Dictionary<Month, int> monthlyStatistics;

        public BudgetItemMonthlyEvolutionDto(BudgetItem budgetItem, Dictionary<Month, int> monthlyStatistics) {
            this.budgetItem = budgetItem;
            this.monthlyStatistics = monthlyStatistics;
        }

        public BudgetItem BudgetItem {
            get { return this.budgetItem; }
            set { this.budgetItem = value; }
        }

        public Dictionary<Month, int> MonthlyStatistics {
            get { return this.monthlyStatistics; }
            set { this.monthlyStatistics = value; }
        }
    }
}
