using System.ComponentModel;

namespace AdvancedBudgetManager.utils.enums {
    public enum PageKey {
        [Description("BudgetSummaryPage")]
        BudgetSummaryPage,

        [Description("IncomesPage")]
        IncomesPage,

        [Description("ExpensesPage")]
        ExpensesPage,

        [Description("DebtsPage")]
        DebtsPage,

        [Description("SavingsPage")]
        SavingsPage,
    }
}
