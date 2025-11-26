using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.utils.database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
   /// <summary>
   /// Repository class for managing the user login operations that require database interaction.
   /// </summary>
    public class UserLoginRepository : ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> {
        private IDatabaseConnection dbConnection;
        private String sqlStatementGetAuthenticationData = @"SELECT userID, username, salt, password FROM users WHERE username = @userName";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginRepository"/> with the provided database connection.
        /// </summary>
        /// <param name="dbConnection">The database connection used for retrieving the data.</param>
        public UserLoginRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        public void DeleteById(long id) {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllByCriteria(UserReadDto getRequest) {
            throw new NotImplementedException();
        }

        public User GetById(long id) {
            throw new NotImplementedException();
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

        public long InsertData(UserInsertDto insertRequest) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UpdateData(IDataUpdateRequest dataUpdateRequest) {
            return;
        }

        public void UpdateData(UserUpdateDto updateRequest) {
            throw new NotImplementedException();
        }
    }
}
