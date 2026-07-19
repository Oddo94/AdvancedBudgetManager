using AdvancedBudgetManagerCore.utils.enums;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for storing the information related to category statistics for different types of budget items.
    /// </summary>
    public class BudgetItemCategoriesStatisticsDto {
        /// <summary>
        /// The budget item.
        /// </summary>
        private BudgetItem budgetItem;

        /// <summary>
        /// The list of <see cref="CategoryStatisticsDto"/> objects containing the actual statistics.
        /// </summary>
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
