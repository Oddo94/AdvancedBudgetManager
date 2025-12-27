using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.security;
using AdvancedBudgetManagerCore.view_model;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service {
    public class LoginUserService {
        /// <summary>
        /// The user repository
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// The security manager used for performing password related operations.
        /// </summary>
        private PasswordSecurityManager securityManager;

        private LoginResponse loginResponse;


        public LoginUserService(IUserRepository userRepository) {
            this.userRepository = userRepository;
            this.securityManager = new PasswordSecurityManager();
        }

        /// <summary>
        /// Checks the user supplied credentials to see if they match the ones stored in the database
        /// </summary>
        /// <exception cref="SystemException"></exception>
        public LoginResponse CheckCredentials([NotNull] UserReadDto userReadDto) {
            if (userReadDto == null) {
                throw new ArgumentException("The searched user cannot be null!");
            }

            User user;
            try {
                user = userRepository.GetByUserName(userReadDto.UserName);
            } catch (SystemException ex) {
                throw new SystemException(ex.Message);
            }

            //FIX AFTER IMPLEMENTING THE USER REGISTRATION SYSTEM!!!

            ////Checks that the user exists and his credentials are correct
            if (user != null && HasValidCredentials(userReadDto, user)) {
                //Sets the login response to success
                loginResponse = new LoginResponse(ResultCode.OK, String.Empty);
            } else {
                loginResponse = new LoginResponse(ResultCode.ERROR, "Invalid username and/or password! Please try again.");
            }

            return loginResponse;
        }

        /// <summary>
        /// Checks if the user supplied credentials match the ones stored in the database.
        /// </summary>
        /// <param name="inputDataTable">The <see cref="DataTable"/> that contains the login credentials.</param>
        /// <param name="userInputPassword">The user supplied password as <see cref="string"/>.</param>
        /// <returns cref="bool"></returns>
        private bool HasValidCredentials([NotNull] UserReadDto providedUser, User actualUser) {
            if (providedUser != null && actualUser != null) {
                //Extracts the stored salt and hashcode for the input password
                byte[] salt = actualUser.Salt;
                String storedHash = actualUser.PasswordHash;

                PasswordSecurityManager securityManager = new PasswordSecurityManager();

                //Generates the hashcode for the input password using the stored salt 
                byte[] actualHashBytes = securityManager.HashSecureString(providedUser.Password, salt);
                string actualHash = securityManager.HashToBase64(actualHashBytes);

                bool isMatch = storedHash.Equals(actualHash);

                salt = null;
                storedHash = null;
                actualHashBytes = null;

                //Checks if the two hashcodes are identical
                //return storedHash.Equals(actualHash);
                return isMatch;
            }

            return false;
        }


        /// <summary>
        /// Sets the <see cref="LoginResponse"/> of the <see cref="LoginViewModel"/>.
        /// </summary>
        public LoginResponse LoginResponse {
            get { return this.loginResponse; }
            set { this.loginResponse = value; }
        }
    }
}
