using System;
using System.Security.Cryptography;
using System.Text;

namespace AdvancedBudgetManagerCore.utils {
    public class PasswordSecurityManager {
        public String createPasswordHash(String password, byte[] salt) {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA512);

            byte[] hashBytes = pbkdf2.GetBytes(32);

            String hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public byte[] getSalt(int size) {
            if (size < 16) {
                return null;
            }
            byte[] salt = new byte[size];

            RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(salt);

            return salt;
        }

        private String convertBinaryToHex(byte[] inputArray) {

            StringBuilder resultArray = new StringBuilder(inputArray.Length * 2);

            foreach (byte currentByte in inputArray) {
                resultArray.AppendFormat("{0:x2}", currentByte);
            }

            return resultArray.ToString();
        }

        //private void writeAuthenticationDataToDB(byte[] salt, String hashedPassword, int userID) {
        //    String sqlStatementInsertAuthenticationData = @"UPDATE users SET users.salt = @paramSalt, users.password = @paramPassword WHERE users.userID = @paramID";
        //    MySqlCommand insertCommand = new MySqlCommand(sqlStatementInsertAuthenticationData);
        //    insertCommand.Parameters.AddWithValue("@paramSalt", salt);
        //    insertCommand.Parameters.AddWithValue("@paramPassword", hashedPassword);
        //    insertCommand.Parameters.AddWithValue("@paramID", userID);

        //    DBConnectionManager.insertData(insertCommand);

        //}

        //private DataTable getData(int userID) {
        //    String sqlStatementGetAuthenticationData = @"SELECT username, salt, password FROM users WHERE userID = @paramID";
        //    MySqlCommand command = new MySqlCommand(sqlStatementGetAuthenticationData);
        //    command.Parameters.AddWithValue("@paramID", userID);

        //    return DBConnectionManager.getData(command);

        //}
    }
}
