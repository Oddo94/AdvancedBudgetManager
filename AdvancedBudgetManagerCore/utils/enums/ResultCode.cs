using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that holds the accepted list of values which can be used to specify the result code value.
    /// </summary>
    public enum ResultCode {
        /// <summary>
        /// Value for the success response
        /// </summary>
        [Description("Ok")]
        Ok,

        /// <summary>
        /// Value for the error response
        /// </summary>
        [Description("Error")]
        Error,

        /// <summary>
        /// Default value to be used when the response code is not known.
        /// </summary>
        [Description("Undefined")]
        Undefined
    }
}
