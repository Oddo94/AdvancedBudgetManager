using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.security;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace AdvancedBudgetManagerCore.repository {
    /// <summary>
    /// Repository class for managing the user reset password operations that require database interaction.
    /// </summary>
    public class ResetPasswordRepository : ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> {
        private IDatabaseConnection dbConnection;
        private PasswordSecurityManager passwordSecurityManager;
        private const int MinimumSaltLength = 32;

        private readonly String sqlStatementUpdateUserPassword = "UPDATE users SET salt = @newSalt, password = @newPassword WHERE userID = (SELECT userID FROM users WHERE email = @userEmail)";

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordRepository"/> with the provided database connection. 
        /// </summary>
        /// <param name="dbConnection"></param>
        public ResetPasswordRepository([NotNull] IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
            this.passwordSecurityManager = new PasswordSecurityManager();
        }

        /// <inheritdoc />
        public DataTable GetData(IDataRequest dataRequest) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        //public void UpdateData(IDataUpdateRequest updateDataRequest) {
        //    PasswordDataUpdateRequest passwordDataUpdateRequest = (PasswordDataUpdateRequest)updateDataRequest;
        //    SecureString newPassword = passwordDataUpdateRequest.NewPassword;
        //    string userEmail = passwordDataUpdateRequest.GetUpdateParameter();

        //    byte[] newSalt = passwordSecurityManager.GetSalt(MinimumSaltLength);
        //    byte[] newPasswordBytes = passwordSecurityManager.HashSecureString(newPassword, newSalt);
        //    string newPasswordHash = passwordSecurityManager.HashToBase64(newPasswordBytes);



        //    using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {

        //        int updateResult = 0;
        //        try {
        //            MySqlCommand updatePasswordCommand = new MySqlCommand(sqlStatementUpdateUserPassword);
        //            updatePasswordCommand.Parameters.AddWithValue("@newSalt", newSalt);
        //            //updatePasswordCommand.Parameters.AddWithValue("@newPassword", newPassword);
        //            updatePasswordCommand.Parameters.AddWithValue("@newPassword", newPasswordHash);
        //            updatePasswordCommand.Parameters.AddWithValue("@userEmail", userEmail);

        //            //MySqlDataAdapter adapter = new MySqlDataAdapter(updatePasswordCommand);

        //            updatePasswordCommand.Connection = conn;
        //            conn.Open();

        //            updateResult = updatePasswordCommand.ExecuteNonQuery();
        //        } catch (MySqlException ex) {
        //            int errorCode = ex.Number;

        //            String errorMessage;
        //            if (errorCode == 1042) {
        //                errorMessage = "Unable to connect to the database! Please check the connection and try again.";
        //            } else {
        //                errorMessage = "An error occurred during the password reset process! Please try again.";
        //            }

        //            throw new SystemException(errorMessage);
        //        } finally {
        //            if (updateResult == 0) {
        //                throw new SystemException("The password could not be updated.");
        //            }
        //        }
        //    }
        //}

        public IEnumerable<User> GetAllByCriteria(UserReadDto getRequest) {
            throw new NotImplementedException();
        }

        public User GetById(long id) {
            throw new NotImplementedException();
        }

        public long InsertData(UserInsertDto insertRequest) {
            throw new NotImplementedException();
        }

        public void UpdateData(UserUpdateDto updateRequest) {
            //PasswordDataUpdateRequest passwordDataUpdateRequest = (PasswordDataUpdateRequest) updateDataRequest;
            //SecureString newPassword = passwordDataUpdateRequest.NewPassword;
            //string userEmail = passwordDataUpdateRequest.GetUpdateParameter();

            //byte[] newSalt = passwordSecurityManager.GetSalt(MinimumSaltLength);
            //byte[] newPasswordBytes = passwordSecurityManager.HashSecureString(newPassword, newSalt);
            //string newPasswordHash = passwordSecurityManager.HashToBase64(newPasswordBytes);


            //FIX AFTER IMPLEMENTING THE USER REGISTRATION SYSTEM!!
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {

                int updateResult = 0;
                try {
                    MySqlCommand updatePasswordCommand = new MySqlCommand(sqlStatementUpdateUserPassword);
                    updatePasswordCommand.Parameters.AddWithValue("@newSalt", null);
                    //updatePasswordCommand.Parameters.AddWithValue("@newPassword", newPassword);
                    updatePasswordCommand.Parameters.AddWithValue("@newPassword", null);
                    updatePasswordCommand.Parameters.AddWithValue("@userEmail", null);

                    //MySqlDataAdapter adapter = new MySqlDataAdapter(updatePasswordCommand);

                    updatePasswordCommand.Connection = conn;
                    conn.Open();

                    updateResult = updatePasswordCommand.ExecuteNonQuery();
                } catch (MySqlException ex) {
                    int errorCode = ex.Number;

                    String errorMessage;
                    if (errorCode == 1042) {
                        errorMessage = "Unable to connect to the database! Please check the connection and try again.";
                    } else {
                        errorMessage = "An error occurred during the password reset process! Please try again.";
                    }

                    throw new SystemException(errorMessage);
                } finally {
                    if (updateResult == 0) {
                        throw new SystemException("The password could not be updated.");
                    }
                }
            }
        }

        public void DeleteById(long id) {
            throw new NotImplementedException();
        }
    }
}
