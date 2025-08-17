using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.view_model {
    //This class acts as a wrapper that contains the view model objects which have to get acces to the user email address
    //Once the email address is inserted in the UI the chared property is updated and, as a result both models with also be updated accordingly
    public partial class ChangePasswordViewModelWrapper : ObservableObject {
        private EmailConfirmationViewModel emailConfirmationViewModel;
        private ResetPasswordViewModel resetPasswordViewModel;

        [ObservableProperty]
        private string userEmail;

        public ChangePasswordViewModelWrapper([NotNull] EmailConfirmationViewModel emailConfirmationViewModel, [NotNull] ResetPasswordViewModel resetPasswordViewModel) {
            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.resetPasswordViewModel = resetPasswordViewModel;
        }

        //This method is called automatically when the userEmail property is changed (due to the automatically generated code by the [ObservableProperty] annotation
        partial void OnUserEmailChanged(string newEmail) {
            //Sets the user email address value to both view models
            this.emailConfirmationViewModel.UserEmail = newEmail;
            this.resetPasswordViewModel.UserEmail = newEmail;
        }
    }
}
