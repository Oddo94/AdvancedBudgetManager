using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.utils.enums;
using Microsoft.Extensions.Configuration;
using System;

namespace AdvancedBudgetManagerCore.utils.security {
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
                    dbConnectionString = string.Empty;
                    break;
            }

            return dbConnectionString;
        }

        /// <summary>
        /// Returns the email sender credentials from the local config file
        /// </summary>
        /// <returns>The <see cref="EmailSenderCredentials"/> object containing the extracted data.</returns>
        public EmailSenderCredentials GetEmailSenderCredentials() {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<SecretReader>()
                .Build();

            string emailSenderAddress = config["EmailSenderAddress"];
            string emailSenderUserName = config["EmailSenderUserName"];
            string emailSenderPassword = config["EmailSenderPassword"];

            return new EmailSenderCredentials(emailSenderAddress, emailSenderUserName, emailSenderPassword);
        }
    }
}
