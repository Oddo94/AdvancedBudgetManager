using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that holds the accepted list of values which can be used to specify the data request type.
    /// </summary>
    public enum DataRequestType {
        /// <summary>
        /// Value for the login data retrieval request
        /// </summary>
        [Description ("LOGIN_DATA_RETRIEVAL")]
        LOGIN_DATA_RETRIEVAL,

        /// <summary>
        /// Value for the budget summary data retrieval request
        /// </summary>
        [Description ("BUDGET_SUMMARY_DATA_RETRIEVAL")]
        BUDGET_SUMMARY_DATA_RETRIEVAL,

        /// <summary>
        /// Default value to be used when the data retrieval request type is not known.
        /// </summary>
        [Description ("UNDEFINED")]
        UNDEFINED
    }
}
