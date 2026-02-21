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
        private static byte[] saltArray = Array.Empty<byte>();
        private static string validEmailAddress = String.Empty;
        private static SecureString validPassword = new SecureString();
        private static SecureString invalidPassword = new SecureString();
        private static string validPasswordString = String.Empty;

        public TestContext TestContext { get; set; }
        public static PasswordSecurityManager? securityManagerInstance;

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            securityManagerInstance = new PasswordSecurityManager();


            validUserId = Convert.ToInt32(testContext.Properties["validUserId"]?.ToString() ?? String.Empty);
            invalidUserId = Convert.ToInt32(testContext.Properties["invalidUserId"]?.ToString() ?? String.Empty);
            validUserName = testContext.Properties["validUserName"]?.ToString() ?? String.Empty;
            invalidUserName = testContext.Properties["invalidUserName"]?.ToString() ?? String.Empty;
            saltArray = Convert.FromBase64String(testContext.Properties["salt"]?.ToString() ?? String.Empty);
            validPasswordHash = testContext.Properties["validPasswordHash"]?.ToString() ?? String.Empty;
            invalidPasswordHash = testContext.Properties["invalidPasswordHash"]?.ToString() ?? String.Empty;
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;
            validPasswordString = testContext.Properties["validPassword"]?.ToString() ?? String.Empty;

            validPassword = securityManagerInstance.ToSecureString(validPasswordHash);
            invalidPassword = securityManagerInstance.ToSecureString(invalidPasswordHash);
        }


        [TestMethod]
        public void CheckCredentials_WhenUserDtoIsNull_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository, securityManager);
            UserReadDto? userReadDto = null;

            Assert.Throws<ArgumentException>(() => loginUserService.CheckCredentials(userReadDto));
        }

        [TestMethod]
        public void CheckCredentials_WhenError_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = new LoginUserService(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, validPassword);
            string errorMessage = "An error occurred while retrieving data! Please try again.";


            userRepository.GetByUserName(Arg.Is<String>(s => s.Length > 0)).Throws(new SystemException(errorMessage));


            SystemException exception = Assert.Throws<SystemException>(() => loginUserService.CheckCredentials(userReadDto));

            Assert.AreEqual(errorMessage, exception.Message);
            userRepository.Received(1).GetByUserName(Arg.Is<String>(s => s.Length > 0));
        }

        [TestMethod]
        public void CheckCredentials_WhenValidCredentials_SuccessfulLogin() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = new LoginUserService(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, validPassword);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);


            userRepository.GetByUserName(Arg.Is<String>(s => s.Length > 0)).Returns(retrievedUser);
            byte[] validPasswordBytes = Convert.FromBase64String(validPasswordHash);
            securityManager.HashSecureString(Arg.Is<SecureString>(s => s.Length > 0), Arg.Is<byte[]>(b => b.SequenceEqual(saltArray))).Returns(validPasswordBytes);
            securityManager.HashToBase64(Arg.Is<byte[]>(b => b.SequenceEqual(validPasswordBytes))).Returns(validPasswordHash);


            GenericResponse response = loginUserService.CheckCredentials(userReadDto);


            Assert.AreEqual(ResultCode.Ok, response.ResultCode);
            Assert.AreEqual(String.Empty, response.ResponseMessage);
            userRepository.Received(1).GetByUserName(Arg.Is<String>(s => s.Length > 0));
            securityManager.Received(1).HashSecureString(Arg.Is<SecureString>(s => s.Length > 0), Arg.Is<byte[]>(b => b.SequenceEqual(saltArray)));
            securityManager.Received(1).HashToBase64(Arg.Is<byte[]>(b => b.SequenceEqual(validPasswordBytes)));
        }

        [TestMethod]
        public void CheckCredentials_WhenInvalidCredentials_FailedLogin() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = new LoginUserService(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(invalidUserName, validPassword);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);
            String errorMessage = "Invalid username and/or password! Please try again.";


            userRepository.GetByUserName(Arg.Is<String>(s => s.Length > 0)).Returns(retrievedUser);
            byte[] invalidPasswordBytes = Convert.FromBase64String(invalidPasswordHash);
            securityManager.HashSecureString(Arg.Is<SecureString>(s => s.Length > 0), Arg.Is<byte[]>(b => b.SequenceEqual(saltArray))).Returns(invalidPasswordBytes);
            securityManager.HashToBase64(Arg.Is<byte[]>(b => b.SequenceEqual(invalidPasswordBytes))).Returns(invalidPasswordHash);


            GenericResponse response = loginUserService.CheckCredentials(userReadDto);


            Assert.AreEqual(ResultCode.Error, response.ResultCode);
            Assert.AreEqual(errorMessage, response.ResponseMessage);
            userRepository.Received(1).GetByUserName(Arg.Is<String>(s => s.Length > 0));
            securityManager.Received(1).HashSecureString(Arg.Is<SecureString>(s => s.Length > 0), Arg.Is<byte[]>(b => b.SequenceEqual(saltArray)));
            securityManager.Received(1).HashToBase64(Arg.Is<byte[]>(b => b.SequenceEqual(invalidPasswordBytes)));
        }

        [TestMethod]
        public void HasValidCredentials_WhenInputUsersAreNull_ReturnFalse() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = new LoginUserService(userRepository, securityManager);


            bool checkResult = loginUserService.HasValidCredentials(null, null);


            Assert.IsFalse(checkResult);
        }

        [TestMethod]
        public void HasValidCredentials_WhenPasswordHashDoesNotMatchStoredHash_ReturnsFalse() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = new LoginUserService(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, invalidPassword);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);


            byte[] invalidPasswordBytes = Convert.FromBase64String(invalidPasswordHash);
            securityManager.HashSecureString(Arg.Is<SecureString>(s => s.Length > 0), Arg.Is<byte[]>(b => b.SequenceEqual(saltArray))).Returns(invalidPasswordBytes);
            securityManager.HashToBase64(Arg.Is<byte[]>(b => b.SequenceEqual(invalidPasswordBytes))).Returns(invalidPasswordHash);

            bool checkResult = loginUserService.HasValidCredentials(userReadDto, retrievedUser);

            Assert.IsFalse(checkResult);
            securityManager.Received(1).HashSecureString(Arg.Is<SecureString>(s => s.Length > 0), Arg.Is<byte[]>(b => b.SequenceEqual(saltArray)));
            securityManager.Received(1).HashToBase64(Arg.Is<byte[]>(b => b.SequenceEqual(invalidPasswordBytes)));
        }

        [TestMethod]
        public void HasValidCredentials_WhenPasswordHashMatchesStoredHash_ReturnsTrue() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = new LoginUserService(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, validPassword);
            User retrievedUser = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress);


            byte[] validPasswordBytes = Convert.FromBase64String(validPasswordHash);
            securityManager.HashSecureString(Arg.Is<SecureString>(s => s.Length > 0), Arg.Is<byte[]>(b => b.SequenceEqual(saltArray))).Returns(validPasswordBytes);
            securityManager.HashToBase64(Arg.Is<byte[]>(b => b.SequenceEqual(validPasswordBytes))).Returns(validPasswordHash);


            bool checkResult = loginUserService.HasValidCredentials(userReadDto, retrievedUser);


            Assert.IsTrue(checkResult);
            securityManager.Received(1).HashSecureString(Arg.Is<SecureString>(s => s.Length > 0), Arg.Is<byte[]>(b => b.SequenceEqual(saltArray)));
            securityManager.Received(1).HashToBase64(Arg.Is<byte[]>(b => b.SequenceEqual(validPasswordBytes)));
        }
    }
}
