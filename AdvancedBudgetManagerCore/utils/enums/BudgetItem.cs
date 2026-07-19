using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that contains the accepted list of values which can be used to specify the budget item.
    /// </summary>
    public enum BudgetItem {
        /// <summary>
        /// Value for the income budget item.
        /// </summary>
        [Description("Income")]
        Income,

        /// <summary>
        /// Value for the expense budget item.
        /// </summary>
        [Description("Expense")]
        Expense,

        /// <summary>
        /// Value for the debt budget item.
        /// </summary>
        [Description("Debt")]
        Debt,

        /// <summary>
        /// Value for the saving budget item.
        /// </summary>
        [Description("Saving")]
        Saving,

        /// <summary>
        /// The default value to be used when the budget item is unknown.
        /// </summary>
        [Description("Undefined")]
        Undefined,
    }
}
