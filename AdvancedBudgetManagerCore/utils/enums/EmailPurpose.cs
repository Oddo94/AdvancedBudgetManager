using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that holds the accepted list of values which can be used to specify the purpose of the sent email. 
    /// </summary>
    public enum EmailPurpose {
        /// <summary>
        /// Value for the user registration email.
        /// </summary>
        [Description("RegisterUserEmail")]
        RegisterUserEmail,

        /// <summary>
        /// Value for the reset password email.
        /// </summary>
        [Description("ResetPasswordEmail")]
        ResetPasswordEmail,

        /// <summary>
        /// Default value to be used when the email purpose is unknown.
        /// </summary>
        [Description("Undefined")]
        Undefined

    }
}
