using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.security;
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
        private string userEmail;

        private ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> resetPasswordRepository;

        private PasswordSecurityManager passwordSecurityManager;
        private const int MinimumSaltLength = 32;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordViewModel"/> based on the provided <see cref="ICrudRepository"/> implementation.
        /// </summary>
        /// <param name="resetPasswordRepository">The actual repository used to reset the user password.</param>
        public ResetPasswordViewModel([NotNull] ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> resetPasswordRepository) {
            this.resetPasswordRepository = resetPasswordRepository;
            this.passwordSecurityManager = new PasswordSecurityManager();
        }

        /// <summary>
        /// Method that performs the password reset operation.
        /// </summary>
        /// <param name="userEmail">The user email for which the password reset was requested.</param>
        [RelayCommand]
        public void ResetPassword([NotNull] string userEmail) {
            bool isSuccess = false;
            string resetPasswordResultMessage = String.Empty;

            //SecureString newPassword = passwordDataUpdateRequest.NewPassword;
            //string userEmail = passwordDataUpdateRequest.GetUpdateParameter();

            /*TO DO
             * Before resetting password:
             * -get the userId by using the email address
             * -create a new UserUpdateDto object based on the existing info (userId, emailAddress) and the newly generated info (salt, password hash)
             * -use the newly created object to update the user data from the DB
             */
            byte[] newSalt = passwordSecurityManager.GetSalt(MinimumSaltLength);
            byte[] newPasswordBytes = passwordSecurityManager.HashSecureString(NewPassword, newSalt);
            string newPasswordHash = passwordSecurityManager.HashToBase64(newPasswordBytes);

            //IDataUpdateRequest passwordUpdateRequest = new PasswordDataUpdateRequest(NewPassword, UserEmail);
            UserUpdateDto userUpdateDto = new UserUpdateDto();

            try {
                resetPasswordRepository.UpdateData(userUpdateDto);

                isSuccess = true;
                resetPasswordResultMessage = "Your password was successfully reset!";
            } catch (SystemException) {
                isSuccess = false;
                resetPasswordResultMessage = "Failed to reset your password. Please try again!";
            }

            WeakReferenceMessenger.Default.Send(new GenericResultMessage(isSuccess, resetPasswordResultMessage));
        }
    }
}