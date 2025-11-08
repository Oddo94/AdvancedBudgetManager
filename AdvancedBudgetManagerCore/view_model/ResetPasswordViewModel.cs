using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.repository;
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

        private ICrudRepository resetPasswordRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordViewModel"/> based on the provided <see cref="ICrudRepository"/> implementation.
        /// </summary>
        /// <param name="resetPasswordRepository">The actual repository used to reset the user password.</param>
        public ResetPasswordViewModel([NotNull] ICrudRepository resetPasswordRepository) {
            this.resetPasswordRepository = resetPasswordRepository;
        }

        /// <summary>
        /// Method that performs the password reset operation.
        /// </summary>
        /// <param name="userEmail">The user email for which the password reset was requested.</param>
        [RelayCommand]
        public void ResetPassword([NotNull] string userEmail) {
            bool isSuccess = false;
            string resetPasswordResultMessage = String.Empty;

            IDataUpdateRequest passwordUpdateRequest = new PasswordDataUpdateRequest(NewPassword, UserEmail);

            try {
                resetPasswordRepository.UpdateData(passwordUpdateRequest);

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