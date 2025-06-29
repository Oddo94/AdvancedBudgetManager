using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that holds the accepted list of values which can be used to specify the current application environment.
    /// </summary>
    public enum AppEnvironment {
        /// <summary>
        /// Value for the production environment.
        /// </summary>
        [Description("PROD")]
        PROD,

        /// <summary>
        /// Value for the test environment.
        /// </summary>
        [Description("TEST")]
        TEST,

        /// <summary>
        /// Value for the development environment.
        /// </summary>
        [Description("DEV")]
        DEV,

        /// <summary>
        /// Default value to be used when the application environment is not known.
        /// </summary>
        [Description("UNDEFINED")]
        UNDEFINED
    }

    /// <summary>
    /// Extension class which provides utility methods for the <see cref="AppEnvironment"/> enum.
    /// </summary>
    public static class AppEnvironmentExtensions {
        /// <summary>
        /// Retrieves an <see cref="AppEnvironment"/> enum value based on its description.
        /// </summary>
        /// <param name="appEnvDescription">The description of the enum value.</param>
        /// <returns></returns>
        public static AppEnvironment GetByDescription(string appEnvDescription) {
            AppEnvironment appEnvironment;

            switch (appEnvDescription) {
                case "PROD":
                    appEnvironment = AppEnvironment.PROD;
                    break;

                case "TEST":
                    appEnvironment = AppEnvironment.TEST;
                    break;

                case "DEV":
                    appEnvironment = AppEnvironment.DEV;
                    break;

                default:
                    appEnvironment = AppEnvironment.UNDEFINED;
                    break;
            }

            return appEnvironment;
        }
    }
}
