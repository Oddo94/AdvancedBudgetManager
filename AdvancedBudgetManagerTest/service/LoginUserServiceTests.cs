using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.security;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Security;

namespace AdvancedBudgetManagerTest.service {
    [TestClass]
    public class LoginUserServiceTests {
        private static long validUserId;
        private static long invalidUserId;
        private static string validUserName = String.Empty;
        private static string invalidUserName = String.Empty;
        private static string validPasswordHash = String.Empty;
        private static string invalidPasswordHash = String.Empty;
        private static string salt = String.Empty;
        private static byte[] saltArray = Array.Empty<byte>();
        private static string validEmailAddress = String.Empty;
        private static SecureString validPassword = new SecureString();
        private static SecureString invalidPassword = new SecureString();

        //private static String invalidPasswordTest = String.Empty;
        private static string validPasswordString = String.Empty;

        public TestContext TestContext { get; set; }
        public static PasswordSecurityManager securityManager;

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            securityManager = new PasswordSecurityManager();


            validUserId = Convert.ToInt32(testContext.Properties["validUserId"]?.ToString() ?? String.Empty);
            invalidUserId = Convert.ToInt32(testContext.Properties["invalidUserId"]?.ToString() ?? String.Empty);
            validUserName = testContext.Properties["validUserName"]?.ToString() ?? String.Empty;
            invalidUserName = testContext.Properties["invalidUserName"]?.ToString() ?? String.Empty;
            saltArray = Convert.FromBase64String(testContext.Properties["salt"]?.ToString() ?? String.Empty);
            validPasswordHash = testContext.Properties["validPasswordHash"]?.ToString() ?? String.Empty;
            invalidPasswordHash = testContext.Properties["invalidPasswordHash"]?.ToString() ?? String.Empty;
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;

            //invalidPasswordTest = testContext.Properties["invalidPassword"]?.ToString() ?? String.Empty;
            validPasswordString = testContext.Properties["validPassword"]?.ToString() ?? String.Empty;

            validPassword = securityManager.ToSecureString(validPasswordHash);
            invalidPassword = securityManager.ToSecureString(invalidPasswordHash);
        }


        [TestMethod]
        public void CheckCredentials_WhenUserDtoIsNull_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository, securityManager);
            UserReadDto userReadDto = null;

            Assert.Throws<ArgumentException>(() => loginUserService.CheckCredentials(userReadDto));
        }

        [TestMethod]
        public void CheckCredentials_WhenError_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, validPassword);

            string errorMessage = "An error occurred while retrieving data! Please try again.";
            userRepository.GetByUserName(userReadDto.UserName).Throws(new SystemException(errorMessage));

            SystemException exception = Assert.Throws<SystemException>(() => loginUserService.CheckCredentials(userReadDto));

            Assert.AreEqual(errorMessage, exception.Message);
        }

        [TestMethod]
        public void CheckCredentials_WhenValidCredentials_SuccessfulLogin() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, validPassword);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);

            userRepository.GetByUserName(userReadDto.UserName).Returns(retrievedUser);
            loginUserService.HasValidCredentials(userReadDto, retrievedUser).Returns(true);
            GenericResponse response = loginUserService.CheckCredentials(userReadDto);

            Assert.AreEqual(ResultCode.OK, response.ResultCode);
            Assert.AreEqual(String.Empty, response.ResponseMessage);
        }

        [TestMethod]
        public void CheckCredentials_WhenInvalidCredentials_FailedLogin() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(invalidUserName, validPassword);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);

            userRepository.GetByUserName(userReadDto.UserName).Returns(retrievedUser);
            loginUserService.HasValidCredentials(userReadDto, retrievedUser).Returns(false);
            GenericResponse response = loginUserService.CheckCredentials(userReadDto);
            String errorMessage = "Invalid username and/or password! Please try again.";

            Assert.AreEqual(ResultCode.ERROR, response.ResultCode);
            Assert.AreEqual(errorMessage, response.ResponseMessage);
        }

        [TestMethod]
        public void HasValidCredentials_WhenInputUsersAreNull_ReturnFalse() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository, securityManager);

            bool checkResult = loginUserService.HasValidCredentials(null, null);

            //Debug.WriteLine($"The password hash for the invalid password is: {securityManager.CreatePasswordHash(invalidPasswordTest, saltArray)}");

            Assert.IsFalse(checkResult);
        }

        [TestMethod]
        public void HasValidCredentials_WhenNoMatch_ReturnFalse() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = new LoginUserService(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, invalidPassword);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);


            byte[] invalidPasswordBytes = Convert.FromBase64String(invalidPasswordHash);
            securityManager.HashSecureString(userReadDto.Password, saltArray).Returns(invalidPasswordBytes);
            securityManager.HashToBase64(invalidPasswordBytes).Returns(invalidPasswordHash);

            bool checkResult = loginUserService.HasValidCredentials(userReadDto, retrievedUser);

            Assert.IsFalse(checkResult);
        }

        [TestMethod]
        public void HasValidCredentials_WhenMatch_ReturnTrue() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = new LoginUserService(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, validPassword);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);


            byte[] validPasswordBytes = Convert.FromBase64String(validPasswordHash);
            securityManager.Received(1).HashSecureString(Arg.Any<SecureString>(), Arg.Any<byte[]>()).Returns(validPasswordBytes);
            securityManager.Received(1).HashToBase64(Arg.Any<byte[]>()).Returns(validPasswordHash);
            //String hashedPasswordTest = Convert.ToBase64String(validPasswordBytes);

            //String generatedHash = securityManager.CreatePasswordHash(validPasswordString, saltArray);

            //bool hashCodesMatch = hashedPasswordTest.Equals(generatedHash);

            bool checkResult = loginUserService.HasValidCredentials(userReadDto, retrievedUser);

            Assert.IsTrue(checkResult);
        }
    }
}
