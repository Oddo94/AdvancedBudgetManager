using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.security;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service {
    /// <summary>
    /// Service class used for performing the operations required during the password reset process.
    /// </summary>
    public class ResetPasswordService {

        /// <summary>
        /// The user repository
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// The security manager used for performing password related operations.
        /// </summary>
        private PasswordSecurityManager securityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordService"/> class based on the provided user repository.
        /// </summary>
        /// <param name="userRepository">The <see cref="IUserRepository"/> instance used for performing the password reset operations.</param>
        public ResetPasswordService(IUserRepository userRepository) {
            this.userRepository = userRepository;
            this.securityManager = new PasswordSecurityManager();
        }

        /// <summary>
        /// Resets the user password based on the provided data.
        /// </summary>
        /// <param name="userUpdateDto">The <see cref="UserUpdateDto"/> instance which contains the data required for the password reset process.</param>
        /// <returns>A <see cref="GenericResponse"/> instance which contains the result of the password reset process.</returns>
        /// <exception cref="ArgumentException"></exception>
        public GenericResponse ResetPassword([NotNull] UserUpdateDto userUpdateDto) {
            if (userUpdateDto == null) {
                throw new ArgumentException("The updated user cannot be null!");
            }

            GenericResponse resetPasswordResponse;

            try {
                //Retrieves the existing user
                User existingUser = userRepository.GetByEmail(userUpdateDto.EmailAddress);

                //Generates a new salt
                byte[] newSalt = securityManager.GetSalt(SecurityConstants.MINIMUM_SALT_LENGTH);

                //Generates a new password hash based on the provided password and the previously generated salt 
                byte[] newPasswordBytes = securityManager.HashSecureString(userUpdateDto.Password, newSalt);
                string newPasswordHash = securityManager.HashToBase64(newPasswordBytes);

                //Creates a new UserUpdateDto instance with all the required data
                User updatedUser = new User(existingUser.UserId, existingUser.UserName, newSalt, newPasswordHash, existingUser.EmailAddress);

                userRepository.Update(updatedUser);

                resetPasswordResponse = new GenericResponse(utils.enums.ResultCode.OK, "Your password was successfully reset!");
            } catch (SystemException) {
                resetPasswordResponse = new GenericResponse(utils.enums.ResultCode.OK, "Failed to reset your password. Please try again!");
            }

            return resetPasswordResponse;
        }
    }
}
