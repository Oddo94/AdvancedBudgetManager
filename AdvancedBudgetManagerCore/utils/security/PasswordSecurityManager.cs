using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace AdvancedBudgetManagerCore.utils.security {
    /// <summary>
    /// Utility class that provides methods for handling password data
    /// </summary>
    public class PasswordSecurityManager {
        /// <summary>
        /// Creates the hash of a password based on the provided salt
        /// </summary>
        /// <param name="password">The password in plain text format</param>
        /// <param name="salt">The salt byte array</param>
        /// <returns cref="string">A <see cref="string"/> representing the computed hash in Base64 format</returns>
        /// <throws cref="ArgumentException"></throws>
        public string CreatePasswordHash([DisallowNull] string password, [DisallowNull] byte[] salt) {
            if (password.Length == 0 || salt.Length == 0) {
                throw new ArgumentException("The salt and password cannot be empty");
            }
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA512);

            byte[] hashBytes = pbkdf2.GetBytes(32);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        /// <summary>
        /// Creates a salt byte array of specified size 
        /// </summary>
        /// <param name="size">The size of the array</param>
        /// <returns>A byte array containing the generated salt</returns>
        /// <throws cref="ArgumentOutOfRangeException"></throws>
        public byte[] GetSalt(int size) {
            if (size < 1 || size > Array.MaxLength) {
                throw new ArgumentOutOfRangeException(string.Format("Invalid array size. Must be between 1 and {0}.", Array.MaxLength));
            }
            byte[] salt = new byte[size];

            RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(salt);

            return salt;
        }
    }
}
