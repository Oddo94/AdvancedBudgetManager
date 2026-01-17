using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that holds the accepted list of values which can be used to specify the purpose of the sent email. 
    /// </summary>
    public enum EmailPurpose {
        /// <summary>
        /// Value for the user registration email.
        /// </summary>
        [Description("REGISTER_USER_EMAIL")]
        REGISTER_USER_EMAIL,

        /// <summary>
        /// Value for the reset password email.
        /// </summary>
        [Description("RESET_PASSWORD_EMAIL")]
        RESET_PASSWORD_EMAIL,

        /// <summary>
        /// Default value to be used when the email purpose is unknown.
        /// </summary>
        [Description("UNDEFINED")]
        UNDEFINED

    }
}
