using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.exception;
using AdvancedBudgetManagerCore.utils.security;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class RegisterUserViewModel : ObservableObject {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private SecureString password;

        [ObservableProperty]
        private string emailAddress;

        private RegisterUserService registerUserService;

        private PasswordSecurityManager securityManager;

        public RegisterUserViewModel([NotNull] RegisterUserService registerUserService) {
            this.registerUserService = registerUserService;
            this.securityManager = new PasswordSecurityManager();
        }

        [RelayCommand]
        public void RegisterUser() {
            string registerUserResultMessage = string.Empty;
            bool isSuccess = false;

            try {
                bool isValidUserData = IsValidUserData(this.UserName, this.EmailAddress);

                if (!isValidUserData) {
                    throw new AdvancedBudgetManagerException("Invalid username and/or email address!");
                }

                UserInsertDto userInsertDto = new UserInsertDto(this.UserName, this.Password, this.EmailAddress);
                registerUserService.RegisterUser(userInsertDto);

                registerUserResultMessage = "Your user was successfully created!";
                isSuccess = true;

            } catch (SystemException) {
                registerUserResultMessage = "Failed to create the new user! Please try again.";
                isSuccess = false;
            }

            WeakReferenceMessenger.Default.Send(new GenericResultMessage(isSuccess, registerUserResultMessage));
        }


        private bool IsValidUserData(string userName, string emailAddress) {
            bool isValidUserData = true;
            bool userExists = false;
            bool isEmailUsed = false;

            try {
                userExists = registerUserService.UserExists(userName);
                isEmailUsed = registerUserService.IsEmailUsed(emailAddress);

                if (userExists || isEmailUsed) {
                    isValidUserData = false;
                }
            } catch (SystemException ex) {
                throw;
            }

            return isValidUserData;
        }
    }
}
