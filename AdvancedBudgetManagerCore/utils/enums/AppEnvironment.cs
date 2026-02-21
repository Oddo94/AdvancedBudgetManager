using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that holds the accepted list of values which can be used to specify the current application environment.
    /// </summary>
    public enum AppEnvironment {
        /// <summary>
        /// Value for the production environment.
        /// </summary>
        [Description("Production")]
        Production,

        /// <summary>
        /// Value for the test environment.
        /// </summary>
        [Description("Test")]
        Test,

        /// <summary>
        /// Value for the development environment.
        /// </summary>
        [Description("Development")]
        Development,

        /// <summary>
        /// Default value to be used when the application environment is not known.
        /// </summary>
        [Description("Undefined")]
        Undefined
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
                case "Production":
                    appEnvironment = AppEnvironment.Production;
                    break;

                case "Test":
                    appEnvironment = AppEnvironment.Test;
                    break;

                case "Development":
                    appEnvironment = AppEnvironment.Development;
                    break;

                default:
                    appEnvironment = AppEnvironment.Undefined;
                    break;
            }

            return appEnvironment;
        }
    }
}
