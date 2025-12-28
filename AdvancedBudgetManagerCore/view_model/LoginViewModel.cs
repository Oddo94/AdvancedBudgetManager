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

        /// <summary>
        /// The object that contains user credentials check result.
        /// </summary>
        public GenericResponse loginResponse;

        /// <summary>
        /// The service that provides the user login operations.
        /// </summary>
        private LoginUserService loginUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> based on the provided <see cref="LoginUserService"/>.
        /// </summary>
        /// <param name="loginUserService">The actual repository used to retrieve the user login details.</param>
        public LoginViewModel([NotNull] LoginUserService loginUserService) {
            this.loginResponse = new GenericResponse();
            this.loginUserService = loginUserService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class with no arguments.
        /// </summary>
        public LoginViewModel() { }

        /// <summary>
        /// Logs in a new user based on the provided credentials.
        /// </summary>
        [RelayCommand]
        public void LoginUser() {
            string loginUserResultMessage = string.Empty;

            try {
                UserReadDto userReadDto = new UserReadDto(this.UserName, this.Password);
                this.loginResponse = loginUserService.CheckCredentials(userReadDto);

            } catch (SystemException ex) {
                this.loginResponse = new GenericResponse(utils.enums.ResultCode.ERROR, $"Failed to login user! Reason: {ex.Message}");
            }
        }
    }
}
