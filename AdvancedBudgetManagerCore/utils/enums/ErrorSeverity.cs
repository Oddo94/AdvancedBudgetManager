using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that hold the accepted list of values which can be used to specify the severity of a specific error.
    /// </summary>
    public enum ErrorSeverity {
        /// <summary>
        /// Value for the INFO level.
        /// </summary>
        [Description("INFO")]
        INFO,

        /// <summary>
        /// Value for the WARNING level.
        /// </summary>
        [Description("WARNING")]
        WARNING,

        /// <summary>
        /// Value for the ERROR level.
        /// </summary>
        [Description("ERROR")]
        ERROR,

        /// <summary>
        /// Value for the CRITICAL level.
        /// </summary>
        [Description("CRITICAL")]
        CRITICAL
    }
}
