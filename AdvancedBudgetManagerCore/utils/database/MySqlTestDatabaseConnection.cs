using MySql.Data.MySqlClient;
using System.Data;

namespace AdvancedBudgetManagerCore.utils.database {
    /// <summary>
    /// Represents a wrapper class for the MySql connections used for testing purposes (e.g. connections to MySql Docker containers for integration testing)
    /// </summary>
    public class MySqlTestDatabaseConnection : IDatabaseConnection {
        /// <summary>
        /// The MySql connection
        /// </summary>
        private MySqlConnection mySqlTestConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlTestDatabaseConnection"/> based on the provided <see cref="MySqlConnection"/> object.
        /// </summary>
        /// <param name="mySqlConnection">The underlying MySql connection.</param>
        public MySqlTestDatabaseConnection(MySqlConnection mySqlConnection) {
            this.mySqlTestConnection = mySqlConnection;
        }

        ///<inheritdoc/>
        public IDbConnection GetConnection() {
            return this.mySqlTestConnection;
        }
    }
}
