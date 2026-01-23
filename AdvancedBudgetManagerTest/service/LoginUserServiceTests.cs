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
using System.Text;

namespace AdvancedBudgetManagerTest.service {
    [TestClass]
    public class LoginUserServiceTests {
        private static long validUserId;
        private static long invalidUserId;
        private static string validUserName = String.Empty;
        private static string invalidUserName = String.Empty;
        private static string passwordHash = String.Empty;
        private static string salt = String.Empty;
        private static byte[] saltArray = Array.Empty<byte>();
        private static string validEmailAddress = String.Empty;
        private static SecureString password = new SecureString();
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
            salt = testContext.Properties["salt"]?.ToString() ?? String.Empty;
            passwordHash = testContext.Properties["passwordHash"]?.ToString() ?? String.Empty;
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;

            password = securityManager.ToSecureString(passwordHash);
            saltArray = salt.Split(",")
             .Select(s => Byte.TryParse(s, out byte b) ? (byte?)b : null)
             .Where(s => s.HasValue)
             .Select(s => s!.Value)
             .ToArray();
        }


        [TestMethod]
        public void CheckCredentials_WhenUserDtoIsNull_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository);
            UserReadDto userReadDto = null;

            Assert.ThrowsException<ArgumentException>(() => loginUserService.CheckCredentials(userReadDto));
        }

        [TestMethod]
        public void CheckCredentials_WhenError_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository);
            UserReadDto userReadDto = new UserReadDto(validUserName, password);

            string errorMessage = "An error occurred while retrieving data! Please try again.";
            userRepository.GetByUserName(userReadDto.UserName).Throws(new SystemException(errorMessage));

            SystemException exception = Assert.ThrowsException<SystemException>(() => loginUserService.CheckCredentials(userReadDto));

            Assert.AreEqual(errorMessage, exception.Message);
        }

        [TestMethod]
        public void CheckCredentials_WhenValidCredentials_SuccessfulLogin() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager securityManager = Substitute.For<PasswordSecurityManager>();
            LoginUserService loginUserService = Substitute.For<LoginUserService>(userRepository, securityManager);
            UserReadDto userReadDto = new UserReadDto(validUserName, password);
            User retrievedUser = new User(validUserId, validUserName, saltArray, passwordHash, validEmailAddress);

            byte[] mockByteArray = Encoding.ASCII.GetBytes(passwordHash);
            securityManager.HashSecureString(password, saltArray).Returns(mockByteArray);
            securityManager.HashToBase64(saltArray).Returns(passwordHash);

            GenericResponse response = loginUserService.CheckCredentials(userReadDto);

            Assert.Equals(ResultCode.OK, response.ResultCode);
            Assert.Equals(String.Empty, response.ResponseMessage);
        }
    }
}
