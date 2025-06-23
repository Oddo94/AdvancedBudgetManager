using AdvancedBudgetManagerCore.model;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils;
using AdvancedBudgetManagerCore.utils.enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Data;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class LoginViewModel : ObservableObject {
        [ObservableProperty]
        private String? userName;

        [ObservableProperty]
        private String? password;

        private LoginResponse loginResponse;

        private int userId;

        private ICrudRepository userLoginRepository;

        public LoginViewModel(ICrudRepository userLoginRepository) {
            this.userName = String.Empty;
            this.password = String.Empty;
            this.loginResponse = new LoginResponse();
            this.userLoginRepository = userLoginRepository;
        }

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
                loginResponse = new LoginResponse(ResultCode.ERROR, "Invalid username and/or password! Please try again");
            }
        }

        public LoginResponse LoginResponse {
            get { return this.loginResponse; }
            set { this.loginResponse = value; }
        }

        public int UserId {
            get { return this.userId; }
        }
        private bool UserExists(DataTable inputDataTable) {
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                return true;
            }

            return false;
        }

        private bool HasValidCredentials(DataTable inputDataTable, String userInputPassword) {
            if (inputDataTable != null && inputDataTable.Rows.Count == 1) {
                //Extracts the stored salt and hashcode for the input password
                byte[] salt = (byte[])inputDataTable.Rows[0].ItemArray[2];
                String storedHash = inputDataTable.Rows[0].ItemArray[3].ToString();

                PasswordSecurityManager securityManager = new PasswordSecurityManager();

                //Generates the hashcode for the input password using the stored salt 
                String actualHash = securityManager.createPasswordHash(userInputPassword, salt);

                //Checks if the two hashcodes are identical
                return storedHash.Equals(actualHash);
            }

            return false;
        }

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
