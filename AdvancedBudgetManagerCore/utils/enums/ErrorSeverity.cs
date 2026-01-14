using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    public enum ErrorSeverity {
        [Description("INFO")]
        INFO,

        [Description("WARNING")]
        WARNING,

        [Description("ERROR")]
        ERROR,

        [Description("CRITICAL")]
        CRITICAL
    }
}
