using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.security;
using NSubstitute;
using System.Security;

namespace AdvancedBudgetManagerTest.service {
    [TestClass]
    public class ResetPasswordServiceTests {
        private static long validUserId = -1;
        private static string validUserName = String.Empty;
        private static string invalidUserName = String.Empty;
        private static string validPasswordHash = String.Empty;
        private static string newPasswordHash = String.Empty;
        private static string invalidPasswordHash = String.Empty;
        private static string validPasswordString = String.Empty;
        private static string newPasswordString = String.Empty;
        private static SecureString validPassword = new SecureString();
        private static SecureString newPassword = new SecureString();
        private static string salt = String.Empty;
        private static byte[] saltArray = Array.Empty<byte>();
        private static byte[] newSaltArray = Array.Empty<byte>();
        private static string validEmailAddress = String.Empty;
        private static string invalidEmailAddress = String.Empty;

        public TestContext TestContext { get; set; }
        public static PasswordSecurityManager securityManagerInstance;

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            securityManagerInstance = new PasswordSecurityManager();

            validUserId = Convert.ToInt32(testContext.Properties["validUserId"]?.ToString() ?? String.Empty);
            validUserName = testContext.Properties["validUserName"]?.ToString() ?? String.Empty;
            invalidUserName = testContext.Properties["invalidUserName"]?.ToString() ?? String.Empty;
            saltArray = Convert.FromBase64String(testContext.Properties["salt"]?.ToString() ?? String.Empty);
            newSaltArray = Convert.FromBase64String(testContext.Properties["newSalt"]?.ToString() ?? String.Empty);
            validPasswordString = testContext.Properties["validPassword"]?.ToString() ?? String.Empty;
            newPasswordString = testContext.Properties["newPassword"]?.ToString() ?? String.Empty;
            validPasswordHash = testContext.Properties["validPasswordHash"]?.ToString() ?? String.Empty;
            newPasswordHash = testContext.Properties["newPasswordHash"]?.ToString() ?? String.Empty;
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;
            invalidEmailAddress = testContext.Properties["invalidEmailAddress"]?.ToString() ?? String.Empty;
            validPassword = securityManagerInstance.ToSecureString(validPasswordString);
            newPassword = securityManagerInstance.ToSecureString(newPasswordString);
        }

        [TestMethod]
        public void ResetPassword_WhenNullUser_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            ResetPasswordService resetPasswordService = new ResetPasswordService(userRepository, securityManager);
            UserUpdateDto userUpdateDto = null;
            string expectedMessage = "The updated user cannot be null!";


            Exception thrownException = Assert.Throws<ArgumentException>(() => resetPasswordService.ResetPassword(userUpdateDto));


            Assert.AreEqual(expectedMessage, thrownException.Message);
        }

        [TestMethod]
        public void ResetPassword_WhenValidEmail_SuccessfulReset() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            ResetPasswordService resetPasswordService = new ResetPasswordService(userRepository, securityManager);
            UserUpdateDto userUpdateDto = new UserUpdateDto(null, null, null, newPassword, validEmailAddress);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);
            User updatedUser = new User(retrievedUser.UserId, retrievedUser.UserName, newSaltArray, newPasswordHash, retrievedUser.EmailAddress);
            ResultCode expectedResultCode = ResultCode.OK;
            string expectedMessage = "Your password was successfully reset!";


            securityManager.GetSalt(SecurityConstants.MINIMUM_SALT_LENGTH).Returns(newSaltArray);
            byte[] newPasswordBytes = securityManager.HashSecureString(userUpdateDto.Password, newSaltArray);
            securityManager.HashSecureString(userUpdateDto.Password, newSaltArray).Returns(newPasswordBytes);
            securityManager.HashToBase64(newPasswordBytes).Returns(newPasswordHash);
            userRepository.GetByEmail(validEmailAddress).Returns(retrievedUser);


            GenericResponse resetPasswordResponse = resetPasswordService.ResetPassword(userUpdateDto);


            userRepository.Received(1).Update(Arg.Is<User>(u =>
                u.UserId == updatedUser.UserId &&
                u.UserName.Equals(updatedUser.UserName) &&
                u.Salt.SequenceEqual(updatedUser.Salt) &&
                u.PasswordHash.Equals(updatedUser.PasswordHash) &&
                u.EmailAddress.Equals(updatedUser.EmailAddress)));
            Assert.AreEqual(expectedResultCode, resetPasswordResponse.ResultCode);
            Assert.AreEqual(expectedMessage, resetPasswordResponse.ResponseMessage);
        }

    }
}
