using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.security;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class RegisterUserViewModel : ObservableObject {
        [ObservableProperty]
        private SecureString password;

        [ObservableProperty]
        private string emailAddress;

        private RegisterUserService registerUserService;

        private PasswordSecurityManager securityManager;

        public RegisterUserViewModel(RegisterUserService registerUserService) {
            this.registerUserService = registerUserService;
            this.securityManager = new PasswordSecurityManager();
        }

        [RelayCommand]
        public void RegisterUser([NotNull] UserInsertDto userInsertDto) {
            string registerUserResultMessage = string.Empty;
            bool isSuccess = false;

            try {
                registerUserService.RegisterUser(userInsertDto);
                registerUserResultMessage = "Your user was successfully created!";
                isSuccess = true;
            } catch (SystemException) {
                registerUserResultMessage = "Failed to create the new user! Please try again.";
                isSuccess = false;
            }

            WeakReferenceMessenger.Default.Send(new GenericResultMessage(isSuccess, registerUserResultMessage));
        }
    }
}
