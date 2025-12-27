using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
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

        public LoginResponse loginResponse;

        private LoginUserService loginUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> based on the provided <see cref="ICrudRepository"/> implementation.
        /// </summary>
        /// <param name="userLoginRepository">The actual repository used to retrieve the user login details.</param>
        public LoginViewModel([NotNull] LoginUserService loginUserService) {
            this.loginResponse = new LoginResponse();
            this.loginUserService = loginUserService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class with no arguments.
        /// </summary>
        public LoginViewModel() { }

        [RelayCommand]
        public void LoginUser() {
            string loginUserResultMessage = string.Empty;

            try {
                UserReadDto userReadDto = new UserReadDto(this.UserName, this.Password);
                this.loginResponse = loginUserService.CheckCredentials(userReadDto);

            } catch (SystemException ex) {
                this.loginResponse = new LoginResponse(utils.enums.ResultCode.ERROR, $"Failed to login user! Reason: {ex.Message}");
            }
        }
    }
}
