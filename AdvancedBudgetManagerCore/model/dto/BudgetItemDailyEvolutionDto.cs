using AdvancedBudgetManagerCore.utils.enums;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.model.dto {
    /// <summary>
    /// Represents the data transfer object used for storing the information related to the daily evolution of a specific budget item..
    /// </summary>
    public class BudgetItemDailyEvolutionDto {
        /// <summary>
        /// The budget item.
        /// </summary>
        private BudgetItem budgetItem;

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the aggreagated daily values for the specified budget item.
        /// </summary>
        private Dictionary<int, int> dailyStatistics;

        /// <summary>
        /// Initalizes a new instance of the <see cref="BudgetItemDailyEvolutionDto"/> based on the provided arguments.
        /// </summary>
        /// <param name="budgetItem">The budget item.</param>
        /// <param name="dailyStatistics"> A <see cref="Dictionary{TKey, TValue}"/> containing the aggreagated daily values for the specified budget item.</param>
        public BudgetItemDailyEvolutionDto(BudgetItem budgetItem, Dictionary<int, int> dailyStatistics) {
            this.budgetItem = budgetItem;
            this.dailyStatistics = dailyStatistics;
        }

        public BudgetItem BudgetItem {
            get { return this.budgetItem; }
            set { this.budgetItem = value; }
        }

        public Dictionary<int, int> DailyStatistics {
            get { return this.dailyStatistics; }
            set { this.dailyStatistics = value; }
        }
    }
}
