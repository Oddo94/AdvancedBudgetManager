using AdvancedBudgetManagerCore.utils.enums;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace AdvancedBudgetManagerCore.utils.database {
    public class MySqlDatabaseConnection : IDatabaseConnection {

        private SecretReader secretReader;

        public MySqlDatabaseConnection() {
            this.secretReader = new SecretReader();
        }


        public IDbConnection GetConnection() {
            //Sets the default value to "TEST" if the extracted value is null
            string appEnvironmentValue = ConfigurationManager.AppSettings["appEnvironment"] ?? "TEST";

            AppEnvironment appEnvironment = AppEnvironmentExtensions.GetByDescription(appEnvironmentValue);

            string dbConnectionString = secretReader.GetSecret(appEnvironment);

            MySqlConnection conn = new MySqlConnection(dbConnectionString);

            return conn;
        }
    }
}
