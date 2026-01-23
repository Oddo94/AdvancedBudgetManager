using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.security;
using AdvancedBudgetManagerCore.view_model;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service {
    /// <summary>
    /// Service class used for performing the repository related operations required during the user login process.
    /// </summary>
    public class LoginUserService {
        /// <summary>
        /// The user repository
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// The security manager used for performing password related operations.
        /// </summary>
        private PasswordSecurityManager securityManager;

        /// <summary>
        /// The object that contains user credentials check result.
        /// </summary>
        private GenericResponse loginResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserService"/> based on the provided user repository.
        /// </summary>
        /// <param name="userRepository">The repository used for retrieving user details</param>
        public LoginUserService(IUserRepository userRepository, PasswordSecurityManager securityManager) {
            this.userRepository = userRepository;
            this.securityManager = securityManager;
        }

        /// <summary>
        /// Checks the user supplied credentials to see if they match the ones stored in the database
        /// </summary>
        /// <param name = "userReadDto">The DTO that contains the login credentials.</param>
        /// <returns>A <see cref="LoginResponse"/> object containing the result of the user credentials check.</returns>
        /// <exception cref="SystemException"></exception>
        public GenericResponse CheckCredentials([NotNull] UserReadDto userReadDto) {
            if (userReadDto == null) {
                throw new ArgumentException("The searched user cannot be null!");
            }

            User user;
            try {
                user = userRepository.GetByUserName(userReadDto.UserName);
            } catch (SystemException ex) {
                throw new SystemException(ex.Message);
            }

            ////Checks that the user exists and his credentials are correct
            if (user != null && HasValidCredentials(userReadDto, user)) {
                //Sets the login response to success
                loginResponse = new GenericResponse(ResultCode.OK, String.Empty);
            } else {
                loginResponse = new GenericResponse(ResultCode.ERROR, "Invalid username and/or password! Please try again.");
            }

            return loginResponse;
        }

        /// <summary>
        /// Checks if the user supplied credentials match the ones stored in the database.
        /// </summary>
        /// <param name="providedUser">The <see cref="UserReadDto"/> that contains the login credentials.</param>
        /// <param name="actualUser">The <see cref="User"/> entity retrieved from the database.</param>
        /// <returns cref="bool"></returns>
        private bool HasValidCredentials([NotNull] UserReadDto providedUser, User actualUser) {
            if (providedUser != null && actualUser != null) {
                //Extracts the stored salt and hashcode for the input password
                byte[] salt = actualUser.Salt;
                String storedHash = actualUser.PasswordHash;

                //PasswordSecurityManager securityManager = new PasswordSecurityManager();

                //Generates the hashcode for the input password using the stored salt 
                byte[] actualHashBytes = securityManager.HashSecureString(providedUser.Password, salt);
                string actualHash = securityManager.HashToBase64(actualHashBytes);

                bool isMatch = storedHash.Equals(actualHash);

                salt = null;
                storedHash = null;
                actualHashBytes = null;

                //Checks if the two hashcodes are identical
                return isMatch;
            }

            return false;
        }


        /// <summary>
        /// Sets the <see cref="LoginResponse"/> of the <see cref="LoginViewModel"/>.
        /// </summary>
        public GenericResponse LoginResponse {
            get { return this.loginResponse; }
            set { this.loginResponse = value; }
        }
    }
}
