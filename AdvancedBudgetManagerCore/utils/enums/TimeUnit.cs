using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that holds the accepted list of values which can be used to specify the time unit.
    /// </summary>
    public enum TimeUnit {
        /// <summary>
        /// Value for the day time unit.
        /// </summary>
        [Description("Day")]
        Day,

        /// <summary>
        /// Value for the month time unit.
        /// </summary>
        [Description("Month")]
        Month,

        /// <summary>
        /// Value for the year time unit.
        /// </summary>
        [Description("Year")]
        Year,

        /// <summary>
        /// Value for an undefined time unit.
        /// </summary>
        [Description("Undefined")]
        Undefined
    }
}
