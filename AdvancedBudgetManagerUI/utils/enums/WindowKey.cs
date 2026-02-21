using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that holds the accepted list of values which can be used to specify the window key.
    /// </summary>
    public enum WindowKey {
        /// <summary>
        /// Value for the reset password window
        /// </summary>
        [Description("ResetPasswordWindow")]
        ResetPasswordWindow,

        /// <summary>
        /// Value for the register user window
        /// </summary>
        [Description("RegisterUserWindow")]
        RegisterUserWindow,

        /// <summary>
        /// Value for the confirm email window
        /// </summary>
        [Description("ConfirmEmailWindow")]
        ConfirmEmailWindow
    }
}
