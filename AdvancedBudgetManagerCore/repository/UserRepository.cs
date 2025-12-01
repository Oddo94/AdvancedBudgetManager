using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public class UserRepository : ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> {
        private IDatabaseConnection dbConnection;
        private String sqlStatementCreateNewUser = @"INSERT INTO users(username, salt, password, email) VALUES(@userName, @salt, @hashCode, @emailAddress)";
        private String sqlStatementGetUserByEmail = "SELECT userID, username, salt, password, email from users where email = @emailAddress";
        private String sqlStatementGetUserByUserName = "SELECT userID, username, salt, password, email from users where username = @userName";
        private String sqlStatementGetUserById = "SELECT userID, username, salt, password, email from users where userId = @userId";

        public UserRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        public User GetById(long id) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getUserByIdCommand = new MySqlCommand(sqlStatementGetUserById, conn);
                    getUserByIdCommand.Parameters.AddWithValue("@userId", id);

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getUserByIdCommand);
                    DataTable userInfo = new DataTable();

                    dataAdapter.Fill(userInfo);

                    long userId;
                    bool parseResult = long.TryParse(userInfo.Rows[0].ItemArray[0].ToString(), out userId);
                    string userName = userInfo.Rows[0].ItemArray[1].ToString();
                    byte[] salt = (byte[])userInfo.Rows[0].ItemArray[2];
                    string storedHash = userInfo.Rows[0].ItemArray[3].ToString();
                    string email = userInfo.Rows[0].ItemArray[4].ToString();

                    User user = new User(userId, userName, salt, storedHash, email);

                    return user;

                } catch (MySqlException ex) {
                    int errorCode = ex.Number;
                    String message;

                    if (errorCode == 1042) {
                        message = "Unable to connect to the database! Please check the connection and try again.";
                    } else {
                        message = "An error occurred while retrieving data! Please try again.";
                    }

                    throw new SystemException(message);
                }
            }
        }

        public User GetByEmail(String emailAddress) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getUserByEmailCommand = new MySqlCommand(sqlStatementGetUserByEmail, conn);
                    getUserByEmailCommand.Parameters.AddWithValue("@emailAddress", emailAddress);

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getUserByEmailCommand);
                    DataTable userInfo = new DataTable();

                    dataAdapter.Fill(userInfo);

                    User user = new User();
                    if (userInfo.Rows.Count > 0) {
                        long userId = -1;
                        bool parseResult = long.TryParse(userInfo.Rows[0].ItemArray[0].ToString(), out userId);
                        string name = userInfo.Rows[0].ItemArray[1].ToString();
                        byte[] salt = (byte[])userInfo.Rows[0].ItemArray[2];
                        string storedHash = userInfo.Rows[0].ItemArray[3].ToString();
                        string email = userInfo.Rows[0].ItemArray[4].ToString();

                        user = new User(userId, name, salt, storedHash, email);
                    } else {
                        user = null;
                    }

                    return user;

                } catch (MySqlException ex) {
                    int errorCode = ex.Number;
                    String message;

                    if (errorCode == 1042) {
                        message = "Unable to connect to the database! Please check the connection and try again.";
                    } else {
                        message = "An error occurred while retrieving data! Please try again.";
                    }

                    throw new SystemException(message);
                }
            }
        }

        public User GetByUserName(String userName) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getUserByUserNameCommand = new MySqlCommand(sqlStatementGetUserByUserName, conn);
                    getUserByUserNameCommand.Parameters.AddWithValue("@userName", userName);

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getUserByUserNameCommand);
                    DataTable userInfo = new DataTable();

                    dataAdapter.Fill(userInfo);

                    User user = new User();

                    if (userInfo.Rows.Count > 0) {
                        long userId = -1;
                        bool parseResult = long.TryParse(userInfo.Rows[0].ItemArray[0].ToString(), out userId);
                        string name = userInfo.Rows[0].ItemArray[1].ToString();
                        byte[] salt = (byte[])userInfo.Rows[0].ItemArray[2];
                        string storedHash = userInfo.Rows[0].ItemArray[3].ToString();
                        string email = userInfo.Rows[0].ItemArray[4].ToString();

                        user = new User(userId, name, salt, storedHash, email);
                    } else {
                        user = null;
                    }

                    return user;

                } catch (MySqlException ex) {
                    int errorCode = ex.Number;
                    String message;

                    if (errorCode == 1042) {
                        message = "Unable to connect to the database! Please check the connection and try again.";
                    } else {
                        message = "An error occurred while retrieving data! Please try again.";
                    }

                    throw new SystemException(message);
                }
            }
        }

        public long InsertData(User user) {
            long generatedUserId;

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

                    return generatedUserId;
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
        }

        public IEnumerable<User> GetAllByCriteria(UserReadDto getRequest) {
            throw new NotImplementedException();
        }

        public void UpdateData(UserUpdateDto updateRequest) {
            throw new NotImplementedException();
        }

        public void DeleteById(long id) {
            throw new NotImplementedException();
        }

        public long InsertData(UserInsertDto insertRequest) {
            throw new NotImplementedException();
        }
    }
}
