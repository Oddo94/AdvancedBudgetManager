using AdvancedBudgetManagerCore.utils;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCoreTest.user_login {
    [TestClass]
    public class UserLoginTests {
        private static string userName;
        private static string password;
        private static string bytes;
        private static string expectedHashCode;

        public TestContext TestContext { get; set; }
        private static PasswordSecurityManager securityManager; 

        public UserLoginTests() {
            userName = String.Empty;
        }

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

           userName = testContext.Properties["userName"].ToString() ?? String.Empty;
            password = testContext.Properties["password"].ToString() ?? String.Empty;
            bytes = testContext.Properties["salt"].ToString() ?? String.Empty;
            expectedHashCode = testContext.Properties["expectedHashCode"].ToString() ?? String.Empty;

            securityManager = new PasswordSecurityManager();
        
              
            }

        [TestMethod]
        public void CreatePasswordHash_WithStoredValues_ReturnsExpectedHash() {
            byte[] salt =  bytes.Split(',')
                 .Select(s => byte.TryParse(s, out byte b) ? (byte?)b : null)
                 .Where(s => s.HasValue)
                 .Select(s => s.Value)
                 .ToArray();

            string actualHashCode = securityManager.CreatePasswordHash(password, salt);
            
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
    }
}
