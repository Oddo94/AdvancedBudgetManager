using System;
using System.Security.Cryptography;

namespace AdvancedBudgetManagerCore.utils {
    /// <summary>
    /// Utility class that provides methods for handling password data
    /// </summary>
    public class PasswordSecurityManager {
        /// <summary>
        /// Creates the hash of a password based on the provided salt
        /// </summary>
        /// <param name="password">The password in plain text format</param>
        /// <param name="salt">The salt byte array</param>
        /// <returns>The computed hash in Base64 format</returns>
        public String createPasswordHash(String password, byte[] salt) {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA512);

            byte[] hashBytes = pbkdf2.GetBytes(32);

            String hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        /// <summary>
        /// Creates a salt byte array of specified size 
        /// </summary>
        /// <param name="size">The size of the array</param>
        /// <returns>A byte array containing the generated salt</returns>
        public byte[] getSalt(int size) {
            if (size < 16) {
                return null;
            }
            byte[] salt = new byte[size];

            RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(salt);

            return salt;
        }
    }
}
