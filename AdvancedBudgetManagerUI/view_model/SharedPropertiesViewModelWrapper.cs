using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.view_model {
#pragma warning disable CS1591
    /// <summary>
    ///  Class that acts as a wrapper that contains the view model objects which have to get acces to the user email address.
    ///  Once the email address is inserted in the UI the shared property is updated and, as a result, both models with also be updated accordingly.
    /// </summary>
    public partial class SharedPropertiesViewModelWrapper : ObservableObject {
        private EmailConfirmationViewModel userRegistrationEmailConfirmationVM;
        private EmailConfirmationViewModel passwordResetEmailConfirmationVM;
        private ResetPasswordViewModel resetPasswordViewModel;
        private RegisterUserViewModel registerUserViewModel;

        [ObservableProperty]
        private string emailAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedPropertiesViewModelWrapper"/> based on the provided <see cref="EmailConfirmationViewModel"/> and <see cref="ResetPasswordViewModel"/> objects.
        /// </summary>
        /// /// <param name="userRegistrationEmailConfirmationVM">The <see cref="EmailConfirmationViewModel"/> instance used for user registration.</param>
        /// <param name="passwordResetEmailConfirmationVM">The <see cref="EmailConfirmationViewModel"/> instance used for password reset.</param>
        /// <param name="resetPasswordViewModel">The <see cref="ResetPasswordViewModel"/> instance.</param>
        /// <param name="registerUserViewModel">The <see cref="RegisterUserViewModel"/> instance.</param>
        public SharedPropertiesViewModelWrapper([NotNull] EmailConfirmationViewModel userRegistrationEmailConfirmationVM,
            [NotNull] EmailConfirmationViewModel passwordResetEmailConfirmationVM,
            [NotNull] ResetPasswordViewModel resetPasswordViewModel,
            [NotNull] RegisterUserViewModel registerUserViewModel) {
            this.userRegistrationEmailConfirmationVM = userRegistrationEmailConfirmationVM;
            this.passwordResetEmailConfirmationVM = passwordResetEmailConfirmationVM;
            this.resetPasswordViewModel = resetPasswordViewModel;
            this.registerUserViewModel = registerUserViewModel;
        }

        /// <summary>
        /// Method called automatically when the userEmail property is changed (due to the automatically generated code by the [ObservableProperty] annotation.
        /// </summary>
        /// <param name="value">The user email.</param>
        partial void OnEmailAddressChanged(string value) {
            //Sets the user email address value to all the provided view models who share this property
            this.userRegistrationEmailConfirmationVM.EmailAddress = value;
            this.passwordResetEmailConfirmationVM.EmailAddress = value;
            this.resetPasswordViewModel.EmailAddress = value;
            this.registerUserViewModel.EmailAddress = value;
        }

        public EmailConfirmationViewModel PasswordResetEmailConfirmationVM {
            get { return this.passwordResetEmailConfirmationVM; }
            set { this.passwordResetEmailConfirmationVM = value; }
        }

        public EmailConfirmationViewModel UserRegistrationEmailConfirmationVM {
            get { return this.userRegistrationEmailConfirmationVM; }
            set { this.userRegistrationEmailConfirmationVM = value; }
        }
    }
}
