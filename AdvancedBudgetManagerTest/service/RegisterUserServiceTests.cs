using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.security;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Security;

namespace AdvancedBudgetManagerTest.service {
    [TestClass]
    public class RegisterUserServiceTests {

        private static string validUserName = String.Empty;
        private static string invalidUserName = String.Empty;
        private static string validPasswordHash = String.Empty;
        private static string invalidPasswordHash = String.Empty;
        private static SecureString validPassword = new SecureString();
        private static string salt = String.Empty;
        private static byte[] saltArray = Array.Empty<byte>();
        private static string validEmailAddress = String.Empty;
        private static string invalidEmailAddress = String.Empty;

        public TestContext TestContext { get; set; }
        public static PasswordSecurityManager securityManager;

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            securityManager = new PasswordSecurityManager();

            validUserName = testContext.Properties["validUserName"]?.ToString() ?? String.Empty;
            invalidUserName = testContext.Properties["invalidUserName"]?.ToString() ?? String.Empty;
            saltArray = Convert.FromBase64String(testContext.Properties["salt"]?.ToString() ?? String.Empty);
            validPasswordHash = testContext.Properties["validPasswordHash"]?.ToString() ?? String.Empty;
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;
            invalidEmailAddress = testContext.Properties["invalidEmailAddress"]?.ToString() ?? String.Empty;
            validPassword = securityManager.ToSecureString(validPasswordHash);

        }

        [TestMethod]
        public void RegisterUser_WhenNullUser_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            UserInsertDto userInsertDto = null;
            string expectedMessage = "The registered user cannot be null!";


            Exception thrownException = Assert.Throws<ArgumentException>(() => registerUserService.RegisterUser(userInsertDto));


            Assert.AreEqual(expectedMessage, thrownException.Message);
        }

        [TestMethod]
        public void RegisterUser_WhenValidUserData_SuccessfulRegistration() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            UserInsertDto userInsertDto = new UserInsertDto(validUserName, validPassword, validEmailAddress);


            passwordSecurityManager.GetSalt(SecurityConstants.MINIMUM_SALT_LENGTH).Returns(saltArray);
            byte[] validPasswordBytes = securityManager.HashSecureString(validPassword, saltArray);
            passwordSecurityManager.HashSecureString(userInsertDto.Password, saltArray).Returns(validPasswordBytes);
            passwordSecurityManager.HashToBase64(validPasswordBytes).Returns(validPasswordHash);
            User user = new User(1, validUserName, saltArray, validPasswordHash, validEmailAddress);
            userRepository.Insert(Arg.Any<User>()).Returns(user);


            registerUserService.RegisterUser(userInsertDto);

            //Checks that the service layer sent the User object with the correct attributes to the repository layer
            userRepository.Received(1).Insert(Arg.Is<User>(u =>
            u.UserName.Equals(user.UserName) &&
            u.Salt.SequenceEqual(user.Salt) &&
            u.PasswordHash.Equals(user.PasswordHash) &&
            u.EmailAddress.Equals(user.EmailAddress)));
        }

        [TestMethod]
        public void RegisterUser_WhenDbError_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            UserInsertDto userInsertDto = new UserInsertDto(validUserName, validPassword, validEmailAddress);
            string expectedMessage = "Unable to connect to the database! Please check the connection and try again.";


            userRepository.Insert(Arg.Any<User>()).Throws(new SystemException(expectedMessage));
            Exception thrownException = Assert.Throws<SystemException>(() => registerUserService.RegisterUser(userInsertDto));


            Assert.AreEqual(expectedMessage, thrownException.Message);
        }

        [TestMethod]
        public void UserExists_WhenNullUser_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            string userName = null;
            string expectedMessage = "The searched username cannot be null!";


            Exception thrownException = Assert.Throws<ArgumentException>(() => registerUserService.UserExists(userName));


            Assert.AreEqual(expectedMessage, thrownException.Message);
        }

        [TestMethod]
        public void UserExists_WhenUserNotExists_ReturnFalse() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            User retrievedUser = null;

            userRepository.GetByUserName(invalidUserName).Returns(retrievedUser);

            Assert.IsFalse(registerUserService.UserExists(invalidUserName));
        }

        [TestMethod]
        public void UserExists_WhenUserExists_ReturnTrue() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            User retrievedUser = new User(1, validUserName, saltArray, validPasswordHash, validEmailAddress);

            userRepository.GetByUserName(validUserName).Returns(retrievedUser);

            Assert.IsTrue(registerUserService.UserExists(validUserName));
        }

        [TestMethod]
        public void UserExists_WhenDbError_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            string expectedMessage = "Unable to connect to the database! Please check the connection and try again.";


            userRepository.GetByUserName(validUserName).Throws(new SystemException(expectedMessage));
            Exception thrownException = Assert.Throws<SystemException>(() => registerUserService.UserExists(validUserName));


            Assert.AreEqual(expectedMessage, thrownException.Message);
        }


        [TestMethod]
        public void IsEmailUsed_WhenNullEmailAddress_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            string emailAddress = null;
            string expectedMessage = "The searched email address cannot be null!";


            Exception thrownException = Assert.Throws<ArgumentException>(() => registerUserService.IsEmailUsed(emailAddress));


            Assert.AreEqual(expectedMessage, thrownException.Message);
        }

        [TestMethod]
        public void IsEmailUsed_WhenEmailNotUsed_ReturnFalse() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            User retrievedUser = null;


            userRepository.GetByEmail(invalidEmailAddress).Returns(retrievedUser);


            Assert.IsFalse(registerUserService.IsEmailUsed(invalidUserName));
        }

        [TestMethod]
        public void IsEmailUsed_WhenEmailUsed_ReturnTrue() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            User retrievedUser = new User(1, validUserName, saltArray, validPasswordHash, validEmailAddress);


            userRepository.GetByEmail(validEmailAddress).Returns(retrievedUser);


            Assert.IsTrue(registerUserService.IsEmailUsed(validEmailAddress));
        }

        [TestMethod]
        public void IsEmailUsed_DbError_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            PasswordSecurityManager passwordSecurityManager = Substitute.For<PasswordSecurityManager>();
            RegisterUserService registerUserService = new RegisterUserService(userRepository, passwordSecurityManager);
            string expectedMessage = "Unable to connect to the database! Please check the connection and try again.";


            userRepository.GetByEmail(validEmailAddress).Throws(new SystemException(expectedMessage));
            Exception thrownException = Assert.Throws<SystemException>(() => userRepository.GetByEmail(validEmailAddress));


            Assert.AreEqual(expectedMessage, thrownException.Message);
        }


    }
}
