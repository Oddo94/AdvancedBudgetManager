using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// Enum class that holds the accepted list of values which can be used to specify the exception category.
    public enum ExceptionCategory {
        /// <summary>
        /// Value for the persistence related exceptions.
        /// </summary>
        [Description("Persistence")]
        Persistence,

        /// <summary>
        /// Value for the data processing related exceptions.
        /// </summary>
        [Description("Data processing")]
        DataProcessing
    }
}
