using AdvancedBudgetManagerCore.utils.enums;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.model.dto {
    public class BudgetItemCategoriesStatisticsDto {
        private BudgetItem budgetItem;
        private List<CategoryStatisticsDto> categoriesStatistics;

        public BudgetItemCategoriesStatisticsDto(BudgetItem budgetItem, List<CategoryStatisticsDto> categoriesStatistics) {
            this.budgetItem = budgetItem;
            this.categoriesStatistics = categoriesStatistics;
        }

        public BudgetItem BudgetItem {
            get { return this.budgetItem; }
            set { this.budgetItem = value; }
        }

        public List<CategoryStatisticsDto> CategoriesStatistics {
            get { return this.categoriesStatistics; }
            set { this.categoriesStatistics = value; }
        }
    }
}
