using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.view_model {
    /// <summary>
    ///  This class acts as a wrapper that contains the view model objects which have to get acces to the user email address.
    ///  Once the email address is inserted in the UI the shared property is updated and, as a result, both models with also be updated accordingly.
    /// </summary>
    public partial class ChangePasswordViewModelWrapper : ObservableObject {
        private EmailConfirmationViewModel emailConfirmationViewModel;
        private ResetPasswordViewModel resetPasswordViewModel;

        [ObservableProperty]
        private string userEmail;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordViewModelWrapper"/> based on the provided <see cref="EmailConfirmationViewModel"/> and <see cref="ResetPasswordViewModel"/> objects.
        /// </summary>
        /// <param name="emailConfirmationViewModel">The <see cref="EmailConfirmationViewModel"/> object.</param>
        /// <param name="resetPasswordViewModel">The <see cref="ResetPasswordViewModel"/> object.</param>
        public ChangePasswordViewModelWrapper([NotNull] EmailConfirmationViewModel emailConfirmationViewModel, [NotNull] ResetPasswordViewModel resetPasswordViewModel) {
            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.resetPasswordViewModel = resetPasswordViewModel;
        }

        /// <summary>
        /// Method called automatically when the userEmail property is changed (due to the automatically generated code by the [ObservableProperty] annotation.
        /// </summary>
        /// <param name="value">The user email.</param>
        partial void OnUserEmailChanged(string value) {
            //Sets the user email address value to both view models
            this.emailConfirmationViewModel.UserEmail = value;
            this.resetPasswordViewModel.UserEmail = value;
        }
    }
}
