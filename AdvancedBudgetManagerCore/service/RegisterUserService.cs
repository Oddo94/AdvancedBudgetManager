using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.security;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service {
    /// <summary>
    /// Service class used for performing the operations required during the user registration process.
    /// </summary>
    public class RegisterUserService {
        /// <summary>
        /// The user repository
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// The security manager used for performing password related operations.
        /// </summary>
        private PasswordSecurityManager securityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserService"/> class based on the provided <see cref="IUserRepository"/> implementation
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public RegisterUserService(IUserRepository userRepository) {
            this.userRepository = userRepository;
            this.securityManager = new PasswordSecurityManager();
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userInsertDto">The DTO that contains the user data.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="SystemException"></exception>
        public void RegisterUser([NotNull] UserInsertDto userInsertDto) {
            if (userInsertDto == null) {
                throw new ArgumentException("The registered user cannot be null!");
            }

            try {

                byte[] salt = securityManager.GetSalt(SecurityConstants.MINIMUM_SALT_LENGTH);
                byte[] passwordBytes = securityManager.HashSecureString(userInsertDto.Password, salt);
                string passwordHash = securityManager.HashToBase64(passwordBytes);

                User user = new User(null, userInsertDto.UserName, salt, passwordHash, userInsertDto.EmailAddress);

                userRepository.Insert(user);

            } catch (SystemException ex) {
                throw new SystemException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Checks if the specified user exists.
        /// </summary>
        /// <param name="userName">The name of the checked user.</param>
        /// <returns>A <see cref="bool"/> value indicating whether the user exists or not.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="SystemException"></exception>
        public bool UserExists([NotNull] string userName) {
            if (userName == null) {
                throw new ArgumentException("The searched username cannot be null!");
            }

            bool userExists = false; ;

            try {
                User user = userRepository.GetByUserName(userName);

                if (user != null) {
                    userExists = true;
                }

            } catch (SystemException ex) {
                throw new SystemException(ex.Message, ex);
            }

            return userExists;
        }

        /// <summary>
        /// Checks if the specified email address is already associated to another user.
        /// </summary>
        /// <param name="emailAddress">The email address that needs to be checked</param>
        /// <returns>A <see cref="bool"/> value indicating whether the email address is used or not.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool IsEmailUsed([NotNull] string emailAddress) {
            if (emailAddress == null) {
                throw new ArgumentException("The searched email address cannot be null!");
            }

            bool isEmailUsed = false;

            try {
                User user = userRepository.GetByEmail(emailAddress);

                if (user != null) {
                    isEmailUsed = true;
                }

            } catch (SystemException ex) {
                throw new SystemException(ex.Message, ex);
            }

            return isEmailUsed;
        }
    }
}
