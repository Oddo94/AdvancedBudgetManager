using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class used for providing information about the request type of the update operation.
    /// </summary>
    public enum DataUpdateRequestType {
        /// <summary>
        /// User password uodate operation.
        /// </summary>
        [Description ("USER_PASSWORD_UPDATE")]
        USER_PASSWORD_UPDATE,

        /// <summary>
        /// Default value for unknown operations.
        /// </summary>
        [Description ("UNDEFINED")]
        UNDEFINED
    }
}
