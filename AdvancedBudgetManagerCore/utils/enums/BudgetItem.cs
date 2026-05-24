using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    public enum BudgetItem {
        [Description("Income")]
        Income,

        [Description("Expense")]
        Expense,

        [Description("Debt")]
        Debt,

        [Description("Saving")]
        Saving,

        [Description("Undefined")]
        Undefined,
    }
}
