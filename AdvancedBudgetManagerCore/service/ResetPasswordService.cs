using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.security;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service {
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
        /// Boolean flag indicating the success/failure of the reset password operation.
        /// </summary>
        private bool isSuccess;

        /// <summary>
        /// The description message for the result of the reset password operation. 
        /// </summary>
        private string resetPasswordResultMessage;

        public ResetPasswordService(IUserRepository userRepository) {
            this.userRepository = userRepository;
            this.securityManager = new PasswordSecurityManager();
        }

        //    public void ResetPassword([NotNull] string userEmail) {
        //        bool isSuccess = false;
        //        string resetPasswordResultMessage = String.Empty;

        //        //SecureString newPassword = passwordDataUpdateRequest.NewPassword;
        //        //string userEmail = passwordDataUpdateRequest.GetUpdateParameter();

        //        /*TO DO
        //         * Before resetting password:
        //         * -get the userId by using the email address
        //         * -create a new UserUpdateDto object based on the existing info (userId, emailAddress) and the newly generated info (salt, password hash)
        //         * -use the newly created object to update the user data from the DB
        //         */
        //        byte[] newSalt = passwordSecurityManager.GetSalt(MinimumSaltLength);
        //        byte[] newPasswordBytes = passwordSecurityManager.HashSecureString(NewPassword, newSalt);
        //        string newPasswordHash = passwordSecurityManager.HashToBase64(newPasswordBytes);

        //        //IDataUpdateRequest passwordUpdateRequest = new PasswordDataUpdateRequest(NewPassword, UserEmail);
        //        UserUpdateDto userUpdateDto = new UserUpdateDto();

        //        try {
        //            //RETRIVE THE CORRECT ENTITY BASED ON THE EMAIL ADDRESS BEFORE PERFORMING THE UPDATE!!
        //            userRepository.Update(null);

        //            isSuccess = true;
        //            resetPasswordResultMessage = "Your password was successfully reset!";
        //        } catch (SystemException) {
        //            isSuccess = false;
        //            resetPasswordResultMessage = "Failed to reset your password. Please try again!";
        //        }

        //        WeakReferenceMessenger.Default.Send(new GenericResultMessage(isSuccess, resetPasswordResultMessage));
        //    }
        //}

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
            } catch (SystemException ex) {
                resetPasswordResponse = new GenericResponse(utils.enums.ResultCode.OK, "Failed to reset your password. Please try again!");
            }

            return resetPasswordResponse;
        }
    }
}
