using AdvancedBudgetManagerCore.utils.enums;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for storing the information related to the monthly evolution of a specific budget item..
    /// </summary>
    public class BudgetItemMonthlyEvolutionDto {
        /// <summary>
        /// The budget item.
        /// </summary>
        private BudgetItem budgetItem;

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the aggreagated monthly values for the specified budget item.
        /// </summary>
        private Dictionary<Month, int> monthlyStatistics;

        /// <summary>
        /// Initalizes a new instance of the <see cref="BudgetItemMonthlyEvolutionDto"/> based on the provided arguments.
        /// </summary>
        /// <param name="budgetItem">The budget item.</param>
        /// <param name="monthlyStatistics"> A <see cref="Dictionary{TKey, TValue}"/> containing the aggreagated monthly values for the specified budget item.</param>
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
