using AdvancedBudgetManagerCore.model;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.view_model;
using NSubstitute;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.OnlineId;
using Windows.Security.Credentials;

namespace AdvancedBudgetManagerCoreTest.user_login {
    [TestClass]
    public class UserLoginTests {
        private static int userId;
        private static string userName;
        private static string password;
        private static string salt;
        private static string expectedHashCode;

        public TestContext TestContext { get; set; }
        private static PasswordSecurityManager securityManager;
        private static LoginViewModel loginViewModel;

        //public UserLoginTests() {
        //    userName = String.Empty;
        //}

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            userId = Convert.ToInt32(testContext.Properties["userId"].ToString());
            userName = testContext.Properties["userName"].ToString() ?? String.Empty;
            password = testContext.Properties["password"].ToString() ?? String.Empty;
            salt = testContext.Properties["salt"].ToString() ?? String.Empty;
            expectedHashCode = testContext.Properties["expectedHashCode"].ToString() ?? String.Empty;

            securityManager = new PasswordSecurityManager();
            loginViewModel = new LoginViewModel();

        
              
            }

        [TestMethod]
        public void CreatePasswordHash_WithStoredValues_ReturnsExpectedHash() {
            byte[] saltArray =  salt.Split(',')
                 .Select(s => byte.TryParse(s, out byte b) ? (byte?)b : null)
                 .Where(s => s.HasValue)
                 .Select(s => s.Value)
                 .ToArray();

            string actualHashCode = securityManager.CreatePasswordHash(password, saltArray);
            
            Assert.AreEqual(expectedHashCode, actualHashCode);
        }

        [TestMethod]
        public void CreatePasswordHash_WithNullParams_ThrowsException() {
            Assert.ThrowsException<NullReferenceException>(() => securityManager.CreatePasswordHash(null, null));
        }

        [TestMethod]
        public void CreatePasswordHash_WithEmptyParams_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => securityManager.CreatePasswordHash(String.Empty, Array.Empty<byte>()));
        }

        [TestMethod]
        public void GetSalt_WithPositiveSize_ReturnsByteArrayOfSpecifiedSize() {
            int expectedSize = 256;

            byte[] salt = securityManager.GetSalt(expectedSize);
            int actualSize = salt.Length;

            Assert.AreEqual(expectedSize, actualSize);
        }

        [TestMethod]
        public void GetSalt_WhenOutOfRangeSize_ThrowException() {
            int size = -1;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => securityManager.GetSalt(size));
        }

        [TestMethod]
        public void CheckCredentials_WhenValidCredentials_ReturnSuccess() {
            IDataRequest loginDataRequest = new UserLoginDataRequest(userName, password);
            DataTable loginResponseDataTable = new DataTable();
            loginResponseDataTable.Columns.Add("userID", typeof(int));
            loginResponseDataTable.Columns.Add("username", typeof(String));
            loginResponseDataTable.Columns.Add("salt", typeof(byte[]));
            loginResponseDataTable.Columns.Add("password", typeof(string));

            byte[] saltArray = salt.Split(",")
                .Select(s => byte.TryParse(s, out byte b) ? (byte?)b : null)
                .Where(s => s.HasValue)
                .Select(s => s.Value)
                .ToArray();

            loginResponseDataTable.Rows.Add(new Object[] { userId, userName, saltArray, expectedHashCode });

            ICrudRepository userLoginRepository = Substitute.For<ICrudRepository>();
            userLoginRepository.GetData(Arg.Is<UserLoginDataRequest>(x => x.UserName == userName && x.Password == password)).Returns(loginResponseDataTable);

            loginViewModel = new LoginViewModel(userLoginRepository);
            loginViewModel.UserName = userName;
            loginViewModel.Password = password;

            loginViewModel.CheckCredentials();

            LoginResponse loginResponse = loginViewModel.LoginResponse;

            Assert.AreEqual(ResultCode.OK, loginResponse.ResultCode);
            Assert.AreEqual(String.Empty, loginResponse.ResponseMessage);
        }

        [TestMethod]
        public void CheckCredentials_WhenInvalidCredentials_ReturnError() {
            ;
            string invalidInputUserName = "Winui4Test";
            string invalidInputPassword = "9AxbgTc4(?{ABC";
            byte[] invalidSalt = new byte[1] { 1 };

            IDataRequest loginDataRequest = new UserLoginDataRequest(userName, password);
            DataTable loginResponseDataTable = new DataTable();
            loginResponseDataTable.Columns.Add("userID", typeof(int));
            loginResponseDataTable.Columns.Add("username", typeof(String));
            loginResponseDataTable.Columns.Add("salt", typeof(byte[]));
            loginResponseDataTable.Columns.Add("password", typeof(string));

            int returnedUserId = -1;
            string returnedUserName = String.Empty;
            byte[] returnedSaltArray = new byte[1] { 1 };
            string returnedPasswordHash = String.Empty;
            loginResponseDataTable.Rows.Add(new Object[] { returnedUserId, returnedUserName, returnedSaltArray, returnedPasswordHash });

            ICrudRepository userLoginRepository = Substitute.For<ICrudRepository>();
            userLoginRepository.GetData(Arg.Is<UserLoginDataRequest>(x => x.UserName == invalidInputUserName && x.Password == invalidInputPassword)).Returns(loginResponseDataTable);

            loginViewModel = new LoginViewModel(userLoginRepository);
            loginViewModel.UserName = invalidInputUserName;
            loginViewModel.Password = invalidInputPassword;

            loginViewModel.CheckCredentials();

            LoginResponse loginResponse = loginViewModel.LoginResponse;

            Assert.AreEqual(ResultCode.ERROR, loginResponse.ResultCode);
            Assert.AreEqual("Invalid username and/or password! Please try again.", loginResponse.ResponseMessage);
        }
    }
}
