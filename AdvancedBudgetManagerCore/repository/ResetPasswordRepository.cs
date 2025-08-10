using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.security;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.repository {
    public class ResetPasswordRepository : ICrudRepository {
        private IDatabaseConnection dbConnection;
        private PasswordSecurityManager passwordSecurityManager;

        private readonly String sqlStatementUpdateUserPassword = "UPDATE users SET salt = @newSalt, password = @newPassword WHERE userID = (SELECT userID FROM users WHERE email = @userEmail)";


        public ResetPasswordRepository([NotNull] IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
            this.passwordSecurityManager = new PasswordSecurityManager();
        }
        public DataTable GetData(IDataRequest dataRequest) {
            throw new NotImplementedException();
        }

        public void UpdateData(IDataUpdateRequest updateDataRequest) {
            PasswordDataUpdateRequest passwordDataUpdateRequest = (PasswordDataUpdateRequest)updateDataRequest;
            string newPassword = passwordDataUpdateRequest.NewPassword;
            string userEmail = passwordDataUpdateRequest.GetUpdateParameter();

            byte[] newSalt = passwordSecurityManager.GetSalt(16);
            String newPasswordHash = passwordSecurityManager.CreatePasswordHash(newPassword, newSalt);



            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {

                int updateResult = 0;
                try {
                    MySqlCommand updatePasswordCommand = new MySqlCommand(sqlStatementUpdateUserPassword);
                    updatePasswordCommand.Parameters.AddWithValue("@newSalt", newSalt);
                    updatePasswordCommand.Parameters.AddWithValue("@newPassword", newPassword);
                    updatePasswordCommand.Parameters.AddWithValue("@userEmail", userEmail);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(updatePasswordCommand);

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
    }
}
