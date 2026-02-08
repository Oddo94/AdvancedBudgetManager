using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    public enum ExceptionCategory {
        [Description("Persistence")]
        Persistence,

        [Description("Data processing")]
        DataProcessing
    }
}
