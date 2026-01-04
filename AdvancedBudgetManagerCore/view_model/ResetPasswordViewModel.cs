using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace AdvancedBudgetManagerCore.view_model {
    /// <summary>
    /// Represents the view model class for the reset password window.
    /// </summary>
    public partial class ResetPasswordViewModel : ObservableObject {
        [ObservableProperty]
        private SecureString newPassword;

        [ObservableProperty]
        private string emailAddress;

        private ResetPasswordService resetPasswordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordViewModel"/> based on the provided <see cref="ResetPasswordService"/>.
        /// </summary>
        /// <param name="resetPasswordService">The actual repository used to reset the user password.</param>
        public ResetPasswordViewModel([NotNull] ResetPasswordService resetPasswordService) {
            this.resetPasswordService = resetPasswordService;
        }

        /// <summary>
        /// Method that performs the password reset operation.
        /// </summary>
        [RelayCommand]
        public void ResetPassword() {
            GenericResponse resetPasswordResponse;
            UserUpdateDto userUpdateDto = new UserUpdateDto(null, null, null, NewPassword, EmailAddress);

            try {
                resetPasswordResponse = resetPasswordService.ResetPassword(userUpdateDto);
            } catch (SystemException) {
                resetPasswordResponse = new GenericResponse(ResultCode.ERROR, "Failed to reset your password. Please try again!");
            }

            WeakReferenceMessenger.Default.Send(new GenericResultMessage(resetPasswordResponse.ResultCode == ResultCode.OK, resetPasswordResponse.ResponseMessage));
        }
    }
}