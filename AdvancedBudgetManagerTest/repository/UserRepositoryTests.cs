using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.database;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AdvancedBudgetManagerTest.repository {
    [TestClass]
    public class UserRepositoryTests {
        private static long validUserId;
        private static long invalidUserId;
        private static string validUserName = String.Empty;
        private static string invalidUserName = String.Empty;
        private static string salt = String.Empty;
        private static string passwordHash = String.Empty;
        private static string validEmailAddress = String.Empty;
        private static string invalidEmailAddress = String.Empty;
        private static string updatedEmailAddress = String.Empty;
        private static byte[] saltArray = Array.Empty<byte>();
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            validUserId = Convert.ToInt32(testContext.Properties["validUserId"]?.ToString() ?? String.Empty);
            invalidUserId = Convert.ToInt32(testContext.Properties["invalidUserId"]?.ToString() ?? String.Empty);
            validUserName = testContext.Properties["validUserName"]?.ToString() ?? String.Empty;
            invalidUserName = testContext.Properties["invalidUserName"]?.ToString() ?? String.Empty;
            salt = testContext.Properties["salt"]?.ToString() ?? String.Empty;
            passwordHash = testContext.Properties["passwordHash"]?.ToString() ?? String.Empty;
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;
            invalidEmailAddress = testContext.Properties["invalidEmailAddress"]?.ToString() ?? String.Empty;
            updatedEmailAddress = testContext.Properties["updatedEmailAddress"]?.ToString() ?? String.Empty;

            saltArray = salt.Split(",")
              .Select(s => Byte.TryParse(s, out byte b) ? (byte?)b : null)
              .Where(s => s.HasValue)
              .Select(s => s!.Value)
              .ToArray();
        }

        [TestMethod]
        public void GetUserById_WhenIdExists_ReturnEntity() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User user = new User(validUserId, validUserName, saltArray, passwordHash, validEmailAddress);
            userRepository.GetById(validUserId).Returns(user);

            User retrievedUser = userRepository.GetById(validUserId);

            Assert.AreEqual(user.UserId, retrievedUser.UserId);
            Assert.AreEqual(user.UserName, retrievedUser.UserName);
            Assert.AreEqual(user.Salt, retrievedUser.Salt);
            Assert.AreEqual(user.PasswordHash, retrievedUser.PasswordHash);
            Assert.AreEqual(user.EmailAddress, retrievedUser.EmailAddress);
        }


        [TestMethod]
        public void GetUserById_WhenIdNotExists_ReturnNull() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User? user = null;
            userRepository.GetById(invalidUserId).Returns(user);

            User retrievedUser = userRepository.GetById(validUserId);

            Assert.IsNull(user);
        }


        [TestMethod]
        public void GetUserById_WhenDbIssue_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            string errorMessage = "Unable to connect to the database! Please check the connection and try again.";
            userRepository.GetById(validUserId).Throws(new SystemException(errorMessage));

            SystemException exception = Assert.ThrowsException<SystemException>(() => userRepository.GetById(validUserId));

            Assert.AreEqual(errorMessage, exception.Message);
        }

        [TestMethod]
        public void GetUserByEmail_WhenEmailExists_ReturnEntity() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User user = new User(validUserId, validUserName, saltArray, passwordHash, validEmailAddress);
            userRepository.GetByEmail(validEmailAddress).Returns(user);

            User retrievedUser = userRepository.GetByEmail(validEmailAddress);

            Assert.AreEqual(user.UserId, retrievedUser.UserId);
            Assert.AreEqual(user.UserName, retrievedUser.UserName);
            Assert.AreEqual(user.Salt, retrievedUser.Salt);
            Assert.AreEqual(user.PasswordHash, retrievedUser.PasswordHash);
            Assert.AreEqual(user.EmailAddress, retrievedUser.EmailAddress);
        }

        [TestMethod]
        public void GetUserByEmail_WhenEmailNotExists_ReturnNull() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User? user = null;
            userRepository.GetByEmail(invalidEmailAddress).Returns(user);

            User retrievedUser = userRepository.GetByEmail(invalidEmailAddress);

            Assert.IsNull(retrievedUser);
        }


        [TestMethod]
        public void GetUserByEmail_WhenDbIssue_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            string errorMessage = "Unable to connect to the database! Please check the connection and try again.";
            userRepository.GetByEmail(validEmailAddress).Throws(new SystemException(errorMessage));

            SystemException exception = Assert.ThrowsException<SystemException>(() => userRepository.GetByEmail(validEmailAddress));

            Assert.AreEqual(errorMessage, exception.Message);
        }

        [TestMethod]
        public void GetUserByUserName_WhenUserNameExists_ReturnEntity() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User user = new User(validUserId, validUserName, saltArray, passwordHash, validEmailAddress);
            userRepository.GetByUserName(validUserName).Returns(user);

            User retrievedUser = userRepository.GetByUserName(validUserName);

            Assert.AreEqual(user.UserId, retrievedUser.UserId);
            Assert.AreEqual(user.UserName, retrievedUser.UserName);
            Assert.AreEqual(user.Salt, retrievedUser.Salt);
            Assert.AreEqual(user.PasswordHash, retrievedUser.PasswordHash);
            Assert.AreEqual(user.EmailAddress, retrievedUser.EmailAddress);
        }

        [TestMethod]
        public void GetUserByUserName_WhenUserNameNotExists_ReturnNull() {
            IDatabaseConnection conn = Substitute.For<IDatabaseConnection>();
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User? user = null;
            userRepository.GetByUserName(invalidUserName).Returns(user);

            User retrievedUser = userRepository.GetByUserName(invalidUserName);

            Assert.IsNull(retrievedUser);
        }


        [TestMethod]
        public void GetUserByUserName_WhenDbIssue_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            string errorMessage = "Unable to connect to the database! Please check the connection and try again.";
            userRepository.GetByUserName(validUserName).Throws(new SystemException(errorMessage));

            SystemException exception = Assert.ThrowsException<SystemException>(() => userRepository.GetByUserName(validUserName));

            Assert.AreEqual(errorMessage, exception.Message);
        }

        [TestMethod]
        public void InsertUser_WhenSuccess_ReturnEntity() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User user = new User(validUserId, validUserName, saltArray, passwordHash, validEmailAddress);
            userRepository.Insert(user).Returns(user);

            User retrievedUser = userRepository.Insert(user);

            Assert.AreEqual(user.UserId, retrievedUser.UserId);
            Assert.AreEqual(user.UserName, retrievedUser.UserName);
            Assert.AreEqual(user.Salt, retrievedUser.Salt);
            Assert.AreEqual(user.PasswordHash, retrievedUser.PasswordHash);
            Assert.AreEqual(user.EmailAddress, retrievedUser.EmailAddress);
        }

        [TestMethod]
        public void InsertUser_WhenError_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User user = new User(validUserId, validUserName, saltArray, passwordHash, validEmailAddress);
            string errorMessage = "An error occurred during user registration! Please try again.";

            userRepository.Insert(user).Throws(new SystemException(errorMessage));

            SystemException exception = Assert.ThrowsException<SystemException>(() => userRepository.Insert(user));

            Assert.AreEqual(errorMessage, exception.Message);
        }

        [TestMethod]
        public void UpdateUser_WhenSuccess_ReturnUpdatedEntity() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User updatedUser = new User(validUserId, validUserName, saltArray, passwordHash, updatedEmailAddress);
            userRepository.Update(updatedUser).Returns(updatedUser);

            User retrievedUser = userRepository.Update(updatedUser);

            Assert.AreEqual(updatedUser.UserId, retrievedUser.UserId);
            Assert.AreEqual(updatedUser.UserName, retrievedUser.UserName);
            Assert.AreEqual(updatedUser.Salt, retrievedUser.Salt);
            Assert.AreEqual(updatedUser.PasswordHash, retrievedUser.PasswordHash);
            Assert.AreEqual(updatedUser.EmailAddress, retrievedUser.EmailAddress);
        }

        [TestMethod]
        public void UpdateUser_WhenError_ThrowException() {
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User updatedUser = new User(validUserId, validUserName, saltArray, passwordHash, updatedEmailAddress);
            string errorMessage = "An error occurred during user update! Please try again.";

            userRepository.Update(updatedUser).Throws(new SystemException(errorMessage));

            SystemException exception = Assert.ThrowsException<SystemException>(() => userRepository.Update(updatedUser));

            Assert.AreEqual(errorMessage, exception.Message);
        }
    }
}
