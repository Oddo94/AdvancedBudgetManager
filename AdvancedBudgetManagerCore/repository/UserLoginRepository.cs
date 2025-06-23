using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.utils.database;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public class UserLoginRepository : ICrudRepository {
        private IDatabaseConnection dbConnection;
        private String sqlStatementGetAuthenticationData = @"SELECT userID, username, salt, password FROM users WHERE username = @userName";

        public UserLoginRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

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
    }
}
