using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.repository;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class ResetPasswordViewModel : ObservableObject {
        [ObservableProperty]
        private string newPassword;

        private string userEmail;

        private ICrudRepository resetPasswordRepository;

        public ResetPasswordViewModel([NotNull] ICrudRepository resetPasswordRepository) {
            this.resetPasswordRepository = resetPasswordRepository;
        }

        public void ResetPassword([NotNull] string userEmail) {
            IDataUpdateRequest passwordUpdateRequest = new PasswordDataUpdateRequest(NewPassword, userEmail);

            try {
                resetPasswordRepository.UpdateData(passwordUpdateRequest);
            } catch(SystemException ex) {
                throw new SystemException($"Password reset failed! Reason: {ex.Message}");
            }
        }
    }
}
