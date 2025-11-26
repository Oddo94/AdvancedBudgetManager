using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.service {
    public class RegisterUserService {
        /* Add the logic for handling salt generation, password generation and repository interaction here*/
        private ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> userRepository;

        private PasswordSecurityManager securityManager;

        public RegisterUserService(ICrudRepository<UserInsertDto, UserReadDto, UserUpdateDto, User, long> userRepository, PasswordSecurityManager securityManager) {
            this.userRepository = userRepository;
            this.securityManager = securityManager;
        }

        public void RegisterUser([NotNull] UserInsertDto userInsertDto) {
            if (userInsertDto == null) {
                throw new ArgumentException("The registered user cannot be null!");
            }

            try {

                byte[] salt = securityManager.GetSalt(SecurityConstants.MINIMUM_SALT_LENGTH);
                byte[] passwordBytes = securityManager.HashSecureString(userInsertDto.Password, salt);
                string passwordHash = securityManager.HashToBase64(passwordBytes);

                User user = new User(null,  userInsertDto.UserName, salt, passwordHash, userInsertDto.EmailAddress);
               
                userRepository.InsertData(userInsertDto);

            } catch (SystemException ex) {
                throw new SystemException(ex.Message, ex);
            } 
        }
    }
}
