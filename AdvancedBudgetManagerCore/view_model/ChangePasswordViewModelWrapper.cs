using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
    /// <summary>
    ///  This class acts as a wrapper that contains the view model objects which have to get acces to the user email address.
    ///  Once the email address is inserted in the UI the shared property is updated and, as a result, both models with also be updated accordingly.
    /// </summary>
    public partial class ChangePasswordViewModelWrapper : ObservableObject {
        private EmailConfirmationViewModel emailConfirmationViewModel;
        private ResetPasswordViewModel resetPasswordViewModel;
        private RegisterUserViewModel registerUserViewModel;

        [ObservableProperty]
        private string emailAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordViewModelWrapper"/> based on the provided <see cref="EmailConfirmationViewModel"/> and <see cref="ResetPasswordViewModel"/> objects.
        /// </summary>
        /// <param name="emailConfirmationViewModel">The <see cref="EmailConfirmationViewModel"/> object.</param>
        /// <param name="resetPasswordViewModel">The <see cref="ResetPasswordViewModel"/> object.</param>
        public ChangePasswordViewModelWrapper([NotNull] EmailConfirmationViewModel emailConfirmationViewModel,
            [NotNull] ResetPasswordViewModel resetPasswordViewModel,
            [NotNull] RegisterUserViewModel registerUserViewModel) {
            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.resetPasswordViewModel = resetPasswordViewModel;
            this.registerUserViewModel = registerUserViewModel;
        }

        /// <summary>
        /// Method called automatically when the userEmail property is changed (due to the automatically generated code by the [ObservableProperty] annotation.
        /// </summary>
        /// <param name="value">The user email.</param>
        partial void OnEmailAddressChanged(string value) {
            //Sets the user email address value to both view models
            this.emailConfirmationViewModel.EmailAddress = value;
            this.resetPasswordViewModel.EmailAddress = value;
            this.registerUserViewModel.EmailAddress = value;
        }
    }
}
