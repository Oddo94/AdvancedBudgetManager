using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.security;
using Autofac.Features.AttributeFilters;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace AdvancedBudgetManagerCore.view_model {
    /// <summary>
    /// Represents the view model class for the login window
    /// </summary>
    public partial class LoginViewModel : ObservableObject {
        [ObservableProperty]
        private String userName;

        [ObservableProperty]
        private SecureString password;

        private LoginResponse loginResponse;

        private int userId;

        private ICrudRepository userLoginRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> based on the provided <see cref="ICrudRepository"/> implementation.
        /// </summary>
        /// <param name="userLoginRepository">The actual repository used to retrieve the user login details.</param>
        public LoginViewModel([NotNull] [KeyFilter("UserLoginRepo")] ICrudRepository userLoginRepository) {
            //this.userName = String.Empty;
            //this.password = String.Empty;
            this.loginResponse = new LoginResponse();
            this.userLoginRepository = userLoginRepository;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class with no arguments.
        /// </summary>
        public LoginViewModel() { }

        /// <summary>
        /// Checks the user supplied credentials to see if they match the ones stored in the database
        /// </summary>
        /// <exception cref="SystemException"></exception>
        public void CheckCredentials() {
            IDataRequest userLoginDataRequest = new UserLoginDataRequest(UserName, Password);

            DataTable authenticationData = new DataTable();
            try {
                authenticationData = userLoginRepository.GetData(userLoginDataRequest);
            } catch (SystemException ex) {
                throw new SystemException(ex.Message);
            }

            //Checks that the user exists and his credentials are correct
            if (UserExists(authenticationData) && HasValidCredentials(authenticationData, Password)) {
                //Sets the login response to success
                loginResponse = new LoginResponse(ResultCode.OK, String.Empty);

                //Extracts the user ID
                userId = GetUserId(authenticationData);
            } else {
                loginResponse = new LoginResponse(ResultCode.ERROR, "Invalid username and/or password! Please try again.");
            }
        }

        /// <summary>
        /// Sets the <see cref="LoginResponse"/> of the <see cref="LoginViewModel"/>.
        /// </summary>
        public LoginResponse LoginResponse {
            get { return this.loginResponse; }
            set { this.loginResponse = value; }
        }

        /// <summary>
        /// Sets the user ID of the <see cref="LoginViewModel"/>.
        /// </summary>
        public int UserId {
            get { return this.userId; }
        }

        /// <summary>
        /// Checks if the user exists based on the specified <see cref="DataTable"/>
        /// </summary>
        /// <param name="inputDataTable">The <see cref="DataTable"/> that contains the login credentials.</param>
        /// <returns cref="bool"></returns>
        private bool UserExists(DataTable inputDataTable) {
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the user supplied credentials match the ones stored in the databse.
        /// </summary>
        /// <param name="inputDataTable">The <see cref="DataTable"/> that contains the login credentials.</param>
        /// <param name="userInputPassword">The user supplied password as <see cref="string"/>.</param>
        /// <returns cref="bool"></returns>
        private bool HasValidCredentials(DataTable inputDataTable, SecureString userInputPassword) {
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                //Extracts the stored salt and hashcode for the input password
                byte[] salt = (byte[])inputDataTable.Rows[0].ItemArray[2];
                String storedHash = inputDataTable.Rows[0].ItemArray[3].ToString();

                PasswordSecurityManager securityManager = new PasswordSecurityManager();

                //Generates the hashcode for the input password using the stored salt 
                //String actualHash = securityManager.CreatePasswordHash(userInputPassword, salt);
                byte[] storedHashBytes = securityManager.HashSecureString(userInputPassword, salt);
                string actualHash = securityManager.HashToBase64(storedHashBytes);

                bool isMatch = storedHash.Equals(actualHash);

                salt = null;
                storedHash = null;
                storedHashBytes = null;

                //Checks if the two hashcodes are identical
                //return storedHash.Equals(actualHash);
                return isMatch;
            }

            return false;
        }

        /// <summary>
        /// Retrives the user ID from the <see cref="DataTable"/> that contains the login credentials.
        /// </summary>
        /// <param name="inputDataTable">The <see cref="DataTable"/> that contains the login credentials.</param>
        /// <returns cref="int"></returns>
        private int GetUserId(DataTable inputDataTable) {
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                Object retrievedID = inputDataTable.Rows[0].ItemArray[0];
                int userID = retrievedID != DBNull.Value ? Convert.ToInt32(retrievedID) : -1;

                return userID;
            }

            return -2;
        }
    }
}
