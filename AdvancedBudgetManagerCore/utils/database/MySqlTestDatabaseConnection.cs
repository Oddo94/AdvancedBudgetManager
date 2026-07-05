using MySql.Data.MySqlClient;
using System.Data;

namespace AdvancedBudgetManagerCore.utils.database {
    public class MySqlTestDatabaseConnection : IDatabaseConnection {
        private MySqlConnection mySqlTestConnection;
        public MySqlTestDatabaseConnection(MySqlConnection mySqlConnection) {
            this.mySqlTestConnection = mySqlConnection;
        }
        public IDbConnection GetConnection() {
            return this.mySqlTestConnection;
        }
    }
}
