using AdvancedBudgetManagerCore.utils.enums;
using Microsoft.Extensions.Configuration;
using System;

namespace AdvancedBudgetManagerCore.utils {
    /// <summary>
    /// Utility class for retrieving data from the user secrets folder of the application
    /// </summary>
    public class SecretReader {

        /// <summary>
        /// Retrieves the database connection string based on the application environment.
        /// </summary>
        /// <param name="appEnvironment">The <see cref="AppEnvironment"/> enum value</param>
        /// <returns>The database connection string assigned to the specified environment</returns>
        public string GetDbConnectionString(AppEnvironment appEnvironment) {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<SecretReader>()
                .Build();

            string dbConnectionString;

            switch (appEnvironment) {
                case AppEnvironment.PROD:
                    dbConnectionString = config["ProdDatabaseConnectionString"];
                    break;

                case AppEnvironment.TEST:
                    dbConnectionString = config["TestDatabaseConnectionString"];
                    break;

                case AppEnvironment.DEV:
                    dbConnectionString = config["DevDatabaseConnectionString"];
                    break;

                default:
                    dbConnectionString = String.Empty;
                    break;
            }

            return dbConnectionString;
        }
    }
}
