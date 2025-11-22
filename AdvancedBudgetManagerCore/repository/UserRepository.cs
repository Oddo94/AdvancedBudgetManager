using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    public class UserRepository : ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> {
        private IDatabaseConnection dbConnection;
        private String sqlStatementCreateNewUser = @"INSERT INTO users(username, salt, password, email) VALUES(@userName, @salt, @hashCode, @emailAddress)";

        public IEnumerable<User> GetAllByCriteria(UserReadDto getRequest) {
            throw new NotImplementedException();
        }

        public User GetById(long id) {
            throw new NotImplementedException();
        }

        public long InsertData(UserInsertDto entity) {
            long generatedUserId = -1;
            UserInsertDto user = (UserInsertDto)entity;
            try {
                using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {

                    MySqlCommand insertUserCommand = new MySqlCommand(sqlStatementCreateNewUser);
                    insertUserCommand.Parameters.AddWithValue("@userName", user.UserName);
                    insertUserCommand.Parameters.AddWithValue("@salt", user.Salt);
                    insertUserCommand.Parameters.AddWithValue("@hashCode", user.PasswordHash);
                    insertUserCommand.Parameters.AddWithValue("@emailAddress", user.EmailAddress);

                    insertUserCommand.Connection = conn;
                    conn.Open();

                    insertUserCommand.ExecuteNonQuery();
                    generatedUserId = insertUserCommand.LastInsertedId;//Check if it returns the correct ID
                }
            } catch (MySqlException ex) {
                int errorCode = ex.ErrorCode;
                String message;

                if (errorCode == 1042) {
                    message = "Unable to connect to the database! Please check the connection and try again.";
                } else {
                    message = "An error occurred during user registration! Please try again.";
                }

                throw new SystemException(message);
            }

       
            return generatedUserId;
        }

        public void UpdateData(UserUpdateDto updateRequest) {
            throw new NotImplementedException();
        }

        public void DeleteById(long id) {
            throw new NotImplementedException();
        }
    }
}
