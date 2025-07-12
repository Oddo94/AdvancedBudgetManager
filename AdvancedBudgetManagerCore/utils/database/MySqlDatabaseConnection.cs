using AdvancedBudgetManagerCore.utils.enums;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace AdvancedBudgetManagerCore.utils.database {
    /// <summary>
    /// Represents a connection to a MySql database.
    /// </summary>
    public class MySqlDatabaseConnection : IDatabaseConnection {

        private SecretReader secretReader;

        /// <summary>
        /// Initializes a new instance of <see cref="MySqlDatabaseConnection"/> with no arguments.
        /// </summary>
        public MySqlDatabaseConnection() {
            this.secretReader = new SecretReader();
        }

        /// <inheritdoc />
        public IDbConnection GetConnection() {
            //Sets the default value to "TEST" if the extracted value is null
            string appEnvironmentValue = ConfigurationManager.AppSettings["appEnvironment"] ?? "TEST";

            AppEnvironment appEnvironment = AppEnvironmentExtensions.GetByDescription(appEnvironmentValue);

            string dbConnectionString = secretReader.GetDbConnectionString(appEnvironment);

            MySqlConnection conn = new MySqlConnection(dbConnectionString);

            return conn;
        }
    }
}
