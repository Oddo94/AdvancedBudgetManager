namespace AdvancedBudgetManagerCore.utils.security {
    /// <summary>
    /// Represents a utility class for storing constants related to data security.
    /// </summary>
    public class SecurityConstants {
        /// <summary>
        /// The minimum salt length.
        /// </summary>
        public static readonly int MINIMUM_SALT_LENGTH = 32;

        /// <summary>
        /// The minimum password length.
        /// </summary>
        public static readonly int MINIMUM_PASSWORD_LENGTH = 12;
    }
}
