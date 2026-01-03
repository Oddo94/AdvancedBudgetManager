using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    public enum EmailPurpose {
        [Description("REGISTER_USER_EMAIL")]
        REGISTER_USER_EMAIL,

        [Description("RESET_PASSWORD_EMAIL")]
        RESET_PASSWORD_EMAIL,

        [Description("UNDEFINED")]
        UNDEFINED

    }
}
