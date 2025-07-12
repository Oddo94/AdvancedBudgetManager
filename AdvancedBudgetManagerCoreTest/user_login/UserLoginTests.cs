﻿using AdvancedBudgetManagerCore.model;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.view_model;
using NSubstitute;
using System.Data;

namespace AdvancedBudgetManagerCoreTest.user_login {
    [TestClass]
    public class UserLoginTests {
        private static int userId;
        private static string validUserName = String.Empty;
        private static string invalidUserName = String.Empty;
        private static string validPassword = String.Empty;
        private static string invalidPassword = String.Empty;
        private static string salt = String.Empty;
        private static string expectedHashCode = String.Empty;

        public TestContext TestContext { get; set; }
        private static PasswordSecurityManager securityManager = new PasswordSecurityManager();


        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            userId = Convert.ToInt32(testContext.Properties["userId"]?.ToString() ?? String.Empty);
            validUserName = testContext.Properties["validUserName"]?.ToString() ?? String.Empty;
            invalidUserName = testContext.Properties["invalidUserName"]?.ToString() ?? String.Empty;
            validPassword = testContext.Properties["validPassword"]?.ToString() ?? String.Empty;
            invalidPassword = testContext.Properties["invalidPassword"]?.ToString() ?? String.Empty;
            salt = testContext.Properties["salt"]?.ToString() ?? String.Empty;
            expectedHashCode = testContext.Properties["expectedHashCode"]?.ToString() ?? String.Empty;

            securityManager = new PasswordSecurityManager();
        }

        [TestMethod]
        public void CreatePasswordHash_WithStoredValues_ReturnsExpectedHash() {
            byte[] saltArray = salt.Split(',')
                 .Select(s => byte.TryParse(s, out byte b) ? (byte?)b : null)
                 .Where(s => s.HasValue)//Filters null values
                 .Select(s => s!.Value)//Adds null forgiving operator to avoid CS8629 (no null values can reach Select due to the Where condition)
                 .ToArray();

            string actualHashCode = securityManager.CreatePasswordHash(validPassword, saltArray);

            Assert.AreEqual(expectedHashCode, actualHashCode);
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
            IDataRequest loginDataRequest = new UserLoginDataRequest(validUserName, validPassword);
            DataTable loginResponseDataTable = new DataTable();
            loginResponseDataTable.Columns.Add("userID", typeof(int));
            loginResponseDataTable.Columns.Add("userName", typeof(String));
            loginResponseDataTable.Columns.Add("salt", typeof(byte[]));
            loginResponseDataTable.Columns.Add("password", typeof(string));

            byte[] saltArray = salt.Split(",")
                .Select(s => byte.TryParse(s, out byte b) ? (byte?)b : null)
                .Where(s => s.HasValue)
                .Select(s => s!.Value)
                .ToArray();

            loginResponseDataTable.Rows.Add(new Object[] { userId, validUserName, saltArray, expectedHashCode });

            ICrudRepository userLoginRepository = Substitute.For<ICrudRepository>();
            userLoginRepository.GetData(Arg.Is<UserLoginDataRequest>(x => x.UserName == validUserName && x.Password == validPassword)).Returns(loginResponseDataTable);

            LoginViewModel loginViewModel = new LoginViewModel(userLoginRepository);
            loginViewModel.UserName = validUserName;
            loginViewModel.Password = validPassword;

            loginViewModel.CheckCredentials();

            LoginResponse loginResponse = loginViewModel.LoginResponse;

            Assert.AreEqual(ResultCode.OK, loginResponse.ResultCode);
            Assert.AreEqual(String.Empty, loginResponse.ResponseMessage);
        }

        [TestMethod]
        public void CheckCredentials_WhenInvalidCredentials_ReturnError() {

            //string invalidInputvalidUserName = "Winui4Test";
            //string invalidInputPassword = "9AxbgTc4(?{ABC";
            byte[] invalidSalt = new byte[1] { 1 };

            IDataRequest loginDataRequest = new UserLoginDataRequest(invalidUserName, invalidPassword);
            DataTable loginResponseDataTable = new DataTable();
            loginResponseDataTable.Columns.Add("userID", typeof(int));
            loginResponseDataTable.Columns.Add("validUserName", typeof(String));
            loginResponseDataTable.Columns.Add("salt", typeof(byte[]));
            loginResponseDataTable.Columns.Add("password", typeof(string));

            int returnedUserId = -1;
            string returnedValidUserName = String.Empty;
            byte[] returnedSaltArray = new byte[1] { 1 };
            string returnedPasswordHash = String.Empty;
            loginResponseDataTable.Rows.Add(new Object[] { returnedUserId, returnedValidUserName, returnedSaltArray, returnedPasswordHash });

            ICrudRepository userLoginRepository = Substitute.For<ICrudRepository>();
            userLoginRepository.GetData(Arg.Is<UserLoginDataRequest>(x => x.UserName == invalidUserName && x.Password == invalidPassword)).Returns(loginResponseDataTable);

            LoginViewModel loginViewModel = new LoginViewModel(userLoginRepository);
            loginViewModel.UserName = invalidUserName;
            loginViewModel.Password = invalidPassword;

            loginViewModel.CheckCredentials();

            LoginResponse loginResponse = loginViewModel.LoginResponse;

            Assert.AreEqual(ResultCode.ERROR, loginResponse.ResultCode);
            Assert.AreEqual("Invalid username and/or password! Please try again.", loginResponse.ResponseMessage);
        }
    }
}
