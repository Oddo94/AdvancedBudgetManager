using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using Autofac.Features.AttributeFilters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
    public partial class ResetPasswordViewModel : ObservableObject, IRecipient<ResetPasswordSubmittedMessage> {
        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmationPassword;

        [ObservableProperty]
        private string userEmail;

        private ICrudRepository resetPasswordRepository;

        private ResetPasswordResponse resetPasswordResponse;

        public ResetPasswordViewModel([NotNull] ICrudRepository resetPasswordRepository) {
            this.resetPasswordRepository = resetPasswordRepository;
            WeakReferenceMessenger.Default.Register<ResetPasswordSubmittedMessage>(this);
        }

        public void Receive(ResetPasswordSubmittedMessage message) {
            //ResetPasswordResponse resetPasswordResponse = message.ResetPasswordResponse;
            //string newPassword = resetPasswordResponse.NewPassword;
            //string confirmationPassword = resetPasswordResponse.ConfirmationPassword;

            //this.resetPasswordResponse = message.ResetPasswordResponse;
        }

        [RelayCommand]
        public void ResetPassword([NotNull] string userEmail) {
            //IDataUpdateRequest passwordUpdateRequest = new PasswordDataUpdateRequest(NewPassword, userEmail);

            //try {
            //    resetPasswordRepository.UpdateData(passwordUpdateRequest);
            //} catch(SystemException ex) {
            //    throw new SystemException($"Password reset failed! Reason: {ex.Message}");
            //}

            //IDataUpdateRequest passwordUpdateRequest = new PasswordDataUpdateRequest(NewPassword, userEmail);

            //string newPassword = resetPasswordResponse.NewPassword;
            //string confirmationPassword = resetPasswordResponse.ConfirmationPassword;

            bool isSuccess = false;
            string resetPasswordResultMessage = String.Empty;
            if ((NewPassword != null || ConfirmationPassword != null)) {
                if (newPassword.Equals((confirmationPassword))) {
                    IDataUpdateRequest passwordUpdateRequest = new PasswordDataUpdateRequest(NewPassword, UserEmail);
                    
                    try {
                        resetPasswordRepository.UpdateData(passwordUpdateRequest);

                        isSuccess = true;
                        resetPasswordResultMessage = "Your password was successfully reset!";
                    } catch (SystemException ex) {
                        isSuccess = false;
                        resetPasswordResultMessage = "Failed to reset your password. Please try again!";
                        //throw new SystemException($"Password reset failed! Reason: {ex.Message}");
                    }
                } else {
                    isSuccess = false;
                    resetPasswordResultMessage = "The input password and confirmation password don't match. Please try again!";
                }
            }

            WeakReferenceMessenger.Default.Send(new GenericResultMessage(isSuccess, resetPasswordResultMessage));
        }
    }
}
