using AdvancedBudgetManager.utils;

namespace AdvancedBudgetManagerCoreTest.data_validation {
    [TestClass]
    public class InputDataValidationTests {
        private static string validPasswordInput = String.Empty;
        private static string passwordMissingLowercase = String.Empty;
        private static string passwordMissingUppercase = String.Empty;
        private static string passwordMissingDigits = String.Empty;
        private static string passwordMissingSpecialChars = String.Empty;
        private static string validLengthInput = String.Empty;
        private static string validLengthInputWithExtraChars = String.Empty;
        private static string invalidLengthInput = String.Empty;
        private static string firstParameterForCompare = String.Empty;
        private static string secondParameterForCompare = String.Empty;
        private static string invalidParameterForCompare = String.Empty;
        private static string nonEmptyParameter = String.Empty;
        private static string nonNullInput = String.Empty;
        private static int requiredInputLength;

        public TestContext TestContext { get; set; }
        private static InputDataValidator dataValidator = null!;


        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            validPasswordInput = testContext.Properties["validPasswordInput"]?.ToString() ?? String.Empty;
            passwordMissingLowercase = testContext.Properties["passwordMissingLowercase"]?.ToString() ?? String.Empty;
            passwordMissingUppercase = testContext.Properties["passwordMissingUppercase"]?.ToString() ?? String.Empty;
            passwordMissingDigits = testContext.Properties["passwordMissingDigits"]?.ToString() ?? String.Empty;
            passwordMissingSpecialChars = testContext.Properties["passwordMissingSpecialChars"]?.ToString() ?? String.Empty;
            validLengthInput = testContext.Properties["validLengthInput"]?.ToString() ?? String.Empty;
            validLengthInputWithExtraChars = testContext.Properties["validLengthInputWithExtraChars"]?.ToString() ?? String.Empty;
            invalidLengthInput = testContext.Properties["invalidLengthInput"]?.ToString() ?? String.Empty;
            firstParameterForCompare = testContext.Properties["firstParameterForCompare"]?.ToString() ?? String.Empty;
            secondParameterForCompare = testContext.Properties["secondParameterForCompare"]?.ToString() ?? String.Empty;
            invalidParameterForCompare = testContext.Properties["invalidParameterForCompare"]?.ToString() ?? String.Empty;
            nonEmptyParameter = testContext.Properties["nonEmptyParameter"]?.ToString() ?? String.Empty;
            nonNullInput = testContext.Properties["nonNullInput"]?.ToString() ?? String.Empty;
            requiredInputLength = Convert.ToInt32(testContext.Properties["requiredInputLength"]?.ToString() ?? String.Empty);

            dataValidator = new InputDataValidator();
        }



        [TestMethod]
        public void CheckPassword_WhenValid_ReturnTrue() {
            bool checkResult = dataValidator.IsValidPassword(validPasswordInput);

            Assert.IsTrue(checkResult);
        }


        [TestMethod]
        public void CheckPassword_WhenMissingLowercase_ReturnFalse() {
            bool checkResult = dataValidator.IsValidPassword(passwordMissingLowercase);

            Assert.IsFalse(checkResult);
        }


        [TestMethod]
        public void CheckPassword_WhenMissingUppercase_ReturnFalse() {
            bool checkResult = dataValidator.IsValidPassword(passwordMissingUppercase);

            Assert.IsFalse(checkResult);
        }


        [TestMethod]
        public void CheckPassword_WhenMissingDigits_ReturnFalse() {
            bool checkResult = dataValidator.IsValidPassword(passwordMissingDigits);

            Assert.IsFalse(checkResult);
        }


        [TestMethod]
        public void CheckPassword_WhenMissingSpecialChars_ReturnFalse() {
            bool checkResult = dataValidator.IsValidPassword(passwordMissingSpecialChars);

            Assert.IsFalse(checkResult);
        }

        [TestMethod]
        public void CheckInputLength_WhenMatchesRequiredLength_ReturnTrue() {
            bool checkResult = dataValidator.HasRequiredLength(validLengthInput, requiredInputLength, ComparisonMode.STRICT);

            Assert.IsTrue(checkResult);
        }

        [TestMethod]
        public void CheckInputLength_WhenLongerThanRequiredLengthAndModeIsLenient_ReturnTrue() {
            bool checkResult = dataValidator.HasRequiredLength(validLengthInputWithExtraChars, requiredInputLength, ComparisonMode.LENIENT);

            Assert.IsTrue(checkResult);
        }

        [TestMethod]
        public void CheckInputLength_WhenDoesNotMatchRequiredLength_ReturnFalse() {
            bool checkResult = dataValidator.HasRequiredLength(invalidLengthInput, requiredInputLength, ComparisonMode.STRICT);

            Assert.IsFalse(checkResult);
        }

        [TestMethod]
        public void CheckInputLength_WhenLongerThanRequiredLengthAndModeIsStrict_ReturnFalse() {
            bool checkResult = dataValidator.HasRequiredLength(validLengthInputWithExtraChars, requiredInputLength, ComparisonMode.STRICT);

            Assert.IsFalse(checkResult);
        }


        [TestMethod]
        public void CheckInputLength_WhenNull_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => dataValidator.HasRequiredLength(null!, requiredInputLength, ComparisonMode.STRICT));
        }

        [TestMethod]
        public void CheckNullArgument_WhenNull_ReturnTrue() {
            Assert.IsTrue(dataValidator.IsNull(null!));
        }

        [TestMethod]
        public void CheckNullArgument_WhenNotNull_ReturnFalse() {
            Assert.IsFalse(dataValidator.IsNull(nonNullInput));
        }

        [TestMethod]
        public void CheckIsMatch_WhenIdenticalValues_ReturnTrue() {
            Assert.IsTrue(dataValidator.IsMatch(firstParameterForCompare, secondParameterForCompare));
        }

        [TestMethod]
        public void CheckIsMatch_WhenDifferentValues_ReturnFalse() {
            Assert.IsFalse(dataValidator.IsMatch(firstParameterForCompare, invalidParameterForCompare));
        }

        [TestMethod]
        public void CheckIsMatch_WhenFirstParamNull_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => dataValidator.IsMatch(null!, secondParameterForCompare));
        }

        [TestMethod]
        public void CheckIsEmpty_WhenEmpty_ReturnTrue() {
            Assert.IsTrue(dataValidator.IsEmpty(""));
        }


        [TestMethod]
        public void CheckIsEmpty_WhenNonEmpty_ReturnFalse() {
            Assert.IsFalse(dataValidator.IsEmpty(nonEmptyParameter));
        }

        [TestMethod]
        public void CheckIsEmpty_WhenNull_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => dataValidator.IsEmpty(null!));
        }
    }
}
