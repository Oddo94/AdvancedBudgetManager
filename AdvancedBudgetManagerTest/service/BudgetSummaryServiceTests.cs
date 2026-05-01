using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.exception;
using NSubstitute;

namespace AdvancedBudgetManagerTest.service {
    [TestClass]
    public class BudgetSummaryServiceTests {
        private static long validUserId = -1;
        private static long invalidUserId = -1;
        private static string validUserName = String.Empty;
        private static byte[] saltArray = Array.Empty<byte>();
        private static string validPasswordHash = String.Empty;
        private static string validEmailAddress = String.Empty;
        private static DateTime validStartDate = DateTime.Now;
        private static DateTime validEndDate = DateTime.Now;
        private static DateTime invalidStartDate = DateTime.Now;
        private static DateTime invalidEndDate = DateTime.Now;
        private static List<DailyExpenseTotalDto> dailyTotalsWithData = new List<DailyExpenseTotalDto>();
        private static List<DailyExpenseTotalDto> dailyTotalsNoDataFound = new List<DailyExpenseTotalDto>();
        private static Dictionary<int, double> dailyExpenseTotals = new Dictionary<int, double>();

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            if (testContext == null) {
                Assert.Fail("Failed to retrieve the test data.");
            }

            validUserId = Convert.ToInt32(testContext.Properties["validUserId"]?.ToString() ?? String.Empty);
            invalidUserId = Convert.ToInt32(testContext.Properties["invalidUserId"]?.ToString() ?? String.Empty);
            validUserName = testContext.Properties["validUserName"]?.ToString() ?? String.Empty;
            saltArray = Convert.FromBase64String(testContext.Properties["salt"]?.ToString() ?? String.Empty);
            validPasswordHash = testContext.Properties["validPasswordHash"]?.ToString() ?? String.Empty;
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;
            DateTime.TryParse(testContext.Properties["validStartDate"]?.ToString() ?? String.Empty, out validStartDate);
            DateTime.TryParse(testContext.Properties["validEndDate"]?.ToString() ?? String.Empty, out validEndDate);
            DateTime.TryParse(testContext.Properties["invalidStartDate"]?.ToString() ?? String.Empty, out invalidStartDate);
            DateTime.TryParse(testContext.Properties["invalidEndDate"]?.ToString() ?? String.Empty, out invalidEndDate);

            int daysInMonth = 30;
            for (int i = 1; i <= daysInMonth; i++) {
                DailyExpenseTotalDto dailyTotalDtoWithData = new DailyExpenseTotalDto(i, (i * 20 + 100) / 3.33);
                dailyTotalsWithData.Add(dailyTotalDtoWithData);

                DailyExpenseTotalDto dailyTotalDtoNoDataFound = new DailyExpenseTotalDto(i, 0);
                dailyTotalsNoDataFound.Add(dailyTotalDtoNoDataFound);
            }
        }

        [TestMethod]
        public void GetDailyExpenseTotals_WhenDataFound_ReturnRetrievedData() {
            IIncomeRepository incomeRepository = Substitute.For<IIncomeRepository>();
            IExpenseRepository expenseRepository = Substitute.For<IExpenseRepository>();
            IDebtRepository debtRepository = Substitute.For<IDebtRepository>();
            ISavingRepository savingRepository = Substitute.For<ISavingRepository>();
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);

            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);

            User user = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress); ;
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);
            userRepository.GetById(validUserId).Returns(user);
            expenseRepository.GetDailyExpenseTotalsForDateInterval(validUserId, validStartDate, validEndDate).Returns(dailyTotalsWithData);

            Dictionary<int, double> retrievedExpenseTotals = budgetSummaryService.GetDailyExpenseTotals(validStartDate, validEndDate);

            int expectedElementCount = 30;
            int day = 10;
            double expectedValue = (day * 20 + 100) / 3.33;

            Assert.HasCount(expectedElementCount, retrievedExpenseTotals.ToArray());
            Assert.AreEqual(expectedValue, retrievedExpenseTotals[day]);
        }

        [TestMethod]
        public void GetDailyExpenseTotals_WhenNoDataFound_ReturnDefaultValues() {
            IIncomeRepository incomeRepository = Substitute.For<IIncomeRepository>();
            IExpenseRepository expenseRepository = Substitute.For<IExpenseRepository>();
            IDebtRepository debtRepository = Substitute.For<IDebtRepository>();
            ISavingRepository savingRepository = Substitute.For<ISavingRepository>();
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);

            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);

            User user = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress); ;
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);
            userRepository.GetById(validUserId).Returns(user);
            expenseRepository.GetDailyExpenseTotalsForDateInterval(validUserId, validStartDate, validEndDate).Returns(dailyTotalsNoDataFound);

            Dictionary<int, double> retrievedExpenseTotals = budgetSummaryService.GetDailyExpenseTotals(validStartDate, validEndDate);

            int expectedElementCount = 30;
            int day = 10;
            double expectedValue = 0;

            Assert.HasCount(expectedElementCount, retrievedExpenseTotals.ToArray());
            Assert.AreEqual(expectedValue, retrievedExpenseTotals[day]);
        }

        [TestMethod]
        public void GetDailyExpenseTotals_WhenUserNotFound_ThrowException() {
            IIncomeRepository incomeRepository = Substitute.For<IIncomeRepository>();
            IExpenseRepository expenseRepository = Substitute.For<IExpenseRepository>();
            IDebtRepository debtRepository = Substitute.For<IDebtRepository>();
            ISavingRepository savingRepository = Substitute.For<ISavingRepository>();
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            AuthenticatedUser authenticatedUser = new AuthenticatedUser(invalidUserId, validEmailAddress);

            User user = null;
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);
            userRepository.GetById(invalidUserId).Returns(user);

            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            string expectedMessage = "The supplied user ID is invalid.";

            AdvancedBudgetManagerException exception = Assert.Throws<AdvancedBudgetManagerException>(() => budgetSummaryService.GetDailyExpenseTotals(validStartDate, validEndDate));

            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [TestMethod]
        public void GetDailyExpenseTotals_WhenInvalidDateParams_ThrowException() {
            IIncomeRepository incomeRepository = Substitute.For<IIncomeRepository>();
            IExpenseRepository expenseRepository = Substitute.For<IExpenseRepository>();
            IDebtRepository debtRepository = Substitute.For<IDebtRepository>();
            ISavingRepository savingRepository = Substitute.For<ISavingRepository>();
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);

            User user = new User(validUserId, validUserName, saltArray, validPasswordHash, validEmailAddress); ;
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);
            userRepository.GetById(validUserId).Returns(user);

            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            string expectedMessage = "The start date must be prior to the end date.";

            AdvancedBudgetManagerException exception = Assert.Throws<AdvancedBudgetManagerException>(() => budgetSummaryService.GetDailyExpenseTotals(invalidStartDate, invalidEndDate));

            Assert.AreEqual(expectedMessage, exception.Message);
        }

    }
}
