using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.security;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AdvancedBudgetManagerCore.service {
    public class RegisterUserService {
        /* Add the logic for handling salt generation, password generation and repository interaction here*/
        private UserRepository userRepository;

        private PasswordSecurityManager securityManager;

        public RegisterUserService(ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> userRepository) {
            this.userRepository = (UserRepository)userRepository;
            this.securityManager = new PasswordSecurityManager();
        }

        public void RegisterUser([NotNull] UserInsertDto userInsertDto) {
            if (userInsertDto == null) {
                throw new ArgumentException("The registered user cannot be null!");
            }

            try {

                byte[] salt = securityManager.GetSalt(SecurityConstants.MINIMUM_SALT_LENGTH);
                byte[] passwordBytes = securityManager.HashSecureString(userInsertDto.Password, salt);
                string passwordHash = securityManager.HashToBase64(passwordBytes);

                User user = new User(null, userInsertDto.UserName, salt, passwordHash, userInsertDto.EmailAddress);

                userRepository.InsertData(userInsertDto);

            } catch (SystemException ex) {
                throw new SystemException(ex.Message, ex);
            }
        }

        public bool UserExists([NotNull] string userName) {
            if (userName == null) {
                throw new ArgumentException("The searched username cannot be null!");
            }

            bool userExists = false; ;

            try {
                User user = userRepository.GetByUserName(userName);

                if (user != null) {
                    userExists = true;
                }

            } catch (SystemException ex) {
                throw new SystemException(ex.Message, ex);
            }

            return userExists;
        }

        public bool IsEmailUsed([NotNull] string emailAddress) {
            if (emailAddress == null) {
                throw new ArgumentException("The searched email address cannot be null!");
            }

            bool isEmailUsed = false;

            try {
                User user = userRepository.GetByEmail(emailAddress);

                if (user != null) {
                    isEmailUsed = true;
                }

            } catch (SystemException ex) {
                throw new SystemException(ex.Message, ex);
            }

            return isEmailUsed;
        }
    }
}
