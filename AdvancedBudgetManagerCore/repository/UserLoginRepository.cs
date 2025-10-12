using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.utils.database;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
   /// <summary>
   /// Repository class for managing the user login operations that require database interaction.
   /// </summary>
    public class UserLoginRepository : ICrudRepository {
        private IDatabaseConnection dbConnection;
        private String sqlStatementGetAuthenticationData = @"SELECT userID, username, salt, password FROM users WHERE username = @userName";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginRepository"/> with the provided database connection.
        /// </summary>
        /// <param name="dbConnection">The database connection used for retrieving the data.</param>
        public UserLoginRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        /// <inheritdoc />
        public DataTable GetData(IDataRequest dataRequest) {

            string userName = dataRequest.GetSearchParameter();

            DataTable authenticationData = new DataTable();

            using (MySqlConnection conn = (MySqlConnection) dbConnection.GetConnection()) {
                try {
                    MySqlCommand authenticationDataCommand = new MySqlCommand(sqlStatementGetAuthenticationData);
                    authenticationDataCommand.Parameters.AddWithValue("@userName", userName);
                    authenticationDataCommand.Connection = conn;           

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(authenticationDataCommand);

                    conn.Open();
                    dataAdapter.Fill(authenticationData);
                } catch (MySqlException ex) {
                    int errorCode = ex.Number;
                    String message;

                    if (errorCode == 1042) {
                        message = "Unable to connect to the database! Please check the connection and try again.";
                    } else {
                        message = "An error occurred during the login process! Please try again.";
                    }

                    throw new SystemException(message);
                }
            }

            return authenticationData;
        }

        /// <inheritdoc />
        public void UpdateData(IDataUpdateRequest dataUpdateRequest) {
            return;
        }
    }
}
