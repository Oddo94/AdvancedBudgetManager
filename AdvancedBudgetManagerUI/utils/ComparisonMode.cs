using System.ComponentModel;

namespace AdvancedBudgetManager.utils {
    /// <summary>
    /// Enum that provides values for the type of comparison that needs to be performed. These values represent general guidlines, however their actual interpretation/meaning is left to the client. 
    /// </summary>
    public enum ComparisonMode {
        /// <summary>
        /// Represents a flexible comparison mode which can allow a more diverse range of values to be accepted.
        /// </summary>
        [Description("LENIENT")]
        LENIENT,

        /// <summary>
        /// Represents a more rigid comparison mode which allows only specific values to be accepted.
        /// </summary>
        [Description("STRICT")]
        STRICT
    }
}
