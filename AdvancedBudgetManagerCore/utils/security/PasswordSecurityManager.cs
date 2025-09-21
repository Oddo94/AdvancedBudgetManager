using Microsoft.UI.Xaml.Documents;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
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

        /// <summary>
        /// Creates a SecureString object from a string input.
        /// For security reasons the caller is responsible for clearing the original <see cref="string"/> object whose reference is provided to this method.
        /// </summary>
        /// <param name="input">The <see cref="string"/> object provided as input./></param>
        /// <returns>A <see cref="SecureString"/> object that contains the original data from the input.</returns>
        /// <throws cref="ArgumentException">if the provided input is a null or empty <see cref="string"/></throws>
        public SecureString ToSecureString(string input) {
            if (input == null || input == String.Empty) {
                throw new ArgumentException("Cannot convert null or empty string to SecureString!");
            }

            SecureString securePassword = new SecureString();
            foreach (char c in input) {
                securePassword.AppendChar(c);
            }

            securePassword.MakeReadOnly();

            return securePassword;
        }

        /// <summary>
        /// Hashes the given SecureString using PBKDF2 with SHA512.
        /// </summary>
        /// <param name="secureString">The secure string containing the password (UTF-16 encoded).</param>
        /// <param name="salt">The salt to use. Must be at least 32 bytes.</param>
        /// <returns>The derived key as a byte array.</returns>
        /// <exception cref="ArgumentException">Thrown if inputs are null or invalid.</exception>

        public byte[] HashSecureString([DisallowNull] SecureString secureString, [DisallowNull] byte[] salt) {
            if (secureString == null || secureString.Length == 0) {
                throw new ArgumentException("Cannot hash a null or empty SecureString object!");
            }

            int minimumSaltLength = 32;
            if (salt == null || salt.Length < minimumSaltLength) {
                throw new ArgumentException($"Salt must not be null and its length should be at least {minimumSaltLength} bytes.");
            }

            //Initializes the pointer which will store the content of the SecureString object
            IntPtr unmanagedStringPointer = IntPtr.Zero;

            try {
                //Convert SecureString to unmanaged memory (UTF-16/Unicode)
                unmanagedStringPointer = Marshal.SecureStringToGlobalAllocUnicode(secureString);

                int unmanagedByteArrayLength = secureString.Length * 2;

                unsafe {
                    //Allocate buffer on the stack
                    byte* byteBuffer = stackalloc byte[unmanagedByteArrayLength];

                    //Copy unmanaged data into the stackalloc buffer
                    Buffer.MemoryCopy(
                        source: (void*)unmanagedStringPointer,
                        destination: byteBuffer,
                        destinationSizeInBytes: unmanagedByteArrayLength,
                        sourceBytesToCopy: unmanagedByteArrayLength);

                    //Wrap stack buffer in ReadOnlySpan<byte>
                    ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(byteBuffer, unmanagedByteArrayLength);

                    int keyLength = 64;
                    int iterationsCount = 100_000;

                    //Hash the span using Rfc2898DeriveBytes with SHA512 algorithm
                    return Rfc2898DeriveBytes.Pbkdf2(span, salt, iterationsCount, HashAlgorithmName.SHA512, keyLength);
                }


            } finally {
                //Wipe and free unmanaged memory
                if (unmanagedStringPointer != IntPtr.Zero) {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedStringPointer);
                }
            }
        }

        /// <summary>
        /// Converts the provided <see cref="byte"/> array to a Base64 string.
        /// </summary>
        /// <param name="hash">The input hash represented as a <see cref="byte"/> array.</param>
        /// <returns>The Base64 encoded string.</returns>
        /// <exception cref="ArgumentException">Thrown if the input hash is null.</exception>
        public string HashToBase64(byte[] hash) {
            if (hash == null) {
                throw new ArgumentException("Cannot convert null byte array to Base64 string!");
            }

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Converts the provided <see cref="byte"/> array to a Hexadecimal string.
        /// </summary>
        /// <param name="hash">The input hash represented as a <see cref="byte"/> array.</param>
        /// <returns>The Hexadecimal encoded string.</returns>
        /// <exception cref="ArgumentException">Thrown if the input hash is null.</exception>
        public String HashToHex(byte[] hash) {
            if (hash == null) {
                throw new ArgumentException("Cannot convert null byte array to Hex string!");
            }

            return Convert.ToHexString(hash);
        }
    }
}
