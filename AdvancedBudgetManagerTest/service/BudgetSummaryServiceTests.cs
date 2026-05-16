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
        private static List<Income> incomeListWithData = new List<Income>();
        private static List<Income> incomeListNoDataFound = new List<Income>();
        private static List<Expense> expenseListWithData = new List<Expense>();
        private static List<Expense> expenseListNoDataFound = new List<Expense>();
        private static List<Debt> debtListWithData = new List<Debt>();
        private static List<Debt> debtListNoDataFound = new List<Debt>();
        private static List<Saving> savingListWithData = new List<Saving>();
        private static List<Saving> savingListNoDataFound = new List<Saving>();
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

            int totalItems = 10;
            for (int i = 1; i <= totalItems; i++) {
                Income income = new Income(i, validUserId, "Test income", 1, i * 100, DateTime.Now.AddDays(i));
                incomeListWithData.Add(income);

                Expense expense = new Expense(i, validUserId, "Test expense", 1, i * 30, DateTime.Now.AddDays(i));
                expenseListWithData.Add(expense);

                Debt debt = new Debt(i, validUserId, "Test debt", i * 20, 1, DateTime.Now.AddDays(i));
                debtListWithData.Add(debt);

                Saving saving = new Saving(i, validUserId, "Test saving", i * 50, DateTime.Now.AddDays(i));
                savingListWithData.Add(saving);
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

            User? user = null;
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

        [TestMethod]
        public void GetIncomeStatistics_WhenIncomesFound_ReturnComputedStatistics() {
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
            incomeRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(incomeListWithData);

            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics incomeStatistics = budgetSummaryService.GetIncomeStatistics(validUserId, validStartDate, validEndDate);

            int expectedValue = 5500;
            double expectedPercentage = 100;
            Assert.AreEqual(expectedValue, incomeStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, incomeStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetIncomeStatistics_WhenNoIncomesFound_ReturnDefaultValues() {
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
            incomeRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(incomeListNoDataFound);

            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics incomeStatistics = budgetSummaryService.GetIncomeStatistics(validUserId, validStartDate, validEndDate);

            int expectedValue = 0;
            double expectedPercentage = 100;
            Assert.AreEqual(expectedValue, incomeStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, incomeStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetExpenseStatistics_WhenExpensesFound_ReturnComputedStatistics() {
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
            expenseRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(expenseListWithData);

            int totalIncomes = 5500;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics expenseStatistics = budgetSummaryService.GetExpenseStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 1650;
            double expectedPercentage = 30;
            Assert.AreEqual(expectedValue, expenseStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, expenseStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetExpenseStatistics_WhenNoExpensesFound_ReturnDefaultValues() {
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
            expenseRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(expenseListNoDataFound);

            int totalIncomes = 5500;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics expenseStatistics = budgetSummaryService.GetExpenseStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 0;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, expenseStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, expenseStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetExpenseStatistics_WhenNoExpensesAndIncomesFound_ReturnDefaultValues() {
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
            expenseRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(expenseListNoDataFound);

            int totalIncomes = 0;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics expenseStatistics = budgetSummaryService.GetExpenseStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 0;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, expenseStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, expenseStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetExpenseStatistics_WhenIncomesEqualZero_ReturnZeroExpensePercentage() {
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
            expenseRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(expenseListWithData);

            int totalIncomes = 0;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics expenseStatistics = budgetSummaryService.GetExpenseStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 1650;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, expenseStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, expenseStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetDebtStatistics_WhenDebtsFound_ReturnComputedStatistics() {
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
            debtRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(debtListWithData);

            int totalIncomes = 5500;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics debtStatistics = budgetSummaryService.GetDebtStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 1100;
            double expectedPercentage = 20;
            Assert.AreEqual(expectedValue, debtStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, debtStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetDebtStatistics_WhenNoDebtsFound_ReturnDefaultValues() {
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
            debtRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(debtListNoDataFound);

            int totalIncomes = 5500;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics debtStatistics = budgetSummaryService.GetDebtStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 0;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, debtStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, debtStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetDebtStatistics_WhenNoDebtsAndIncomesFound_ReturnDefaultValues() {
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
            debtRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(debtListNoDataFound);

            int totalIncomes = 0;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics debtStatistics = budgetSummaryService.GetDebtStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 0;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, debtStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, debtStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetDebtStatistics_WhenIncomesEqualZero_ReturnZeroDebtsPercentage() {
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
            debtRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(debtListWithData);

            int totalIncomes = 0;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics debtStatistics = budgetSummaryService.GetDebtStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 1100;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, debtStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, debtStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetSavingStatistics_WhenSavingsFound_ReturnComputedStatistics() {
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
            savingRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(savingListWithData);

            int totalIncomes = 5500;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics savingStatistics = budgetSummaryService.GetSavingStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 2750;
            double expectedPercentage = 50;
            Assert.AreEqual(expectedValue, savingStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, savingStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetSavingStatistics_WhenNoSavingsFound_ReturnDefaultValues() {
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
            savingRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(savingListNoDataFound);

            int totalIncomes = 5500;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics savingStatistics = budgetSummaryService.GetSavingStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 0;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, savingStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, savingStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetSavingStatistics_WhenNoSavingsAndIncomesFound_ReturnDefaultValues() {
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
            savingRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(savingListNoDataFound);

            int totalIncomes = 0;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics savingStatistics = budgetSummaryService.GetSavingStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 0;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, savingStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, savingStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetSavingStatistics_WhenIncomesEqualZero_ReturnZeroSavingsPercentage() {
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
            savingRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(savingListWithData);

            int totalIncomes = 0;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics savingStatistics = budgetSummaryService.GetSavingStatistics(validUserId, validStartDate, validEndDate, totalIncomes);

            int expectedValue = 2750;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, savingStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, savingStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetTotalLeftToSpendStatistics_WhenPositiveIncomes_ReturnComputedStatistics() {
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
            savingRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(savingListWithData);

            int totalIncomes = 5500;
            int totalExpenses = 1650;
            int totalDebts = 1100;
            int totalSavings = 2000;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics leftToSpendStatistics = budgetSummaryService.GetTotalLeftToSpendStatistics(totalIncomes, totalExpenses, totalDebts, totalSavings);

            int expectedValue = 750;
            double expectedPercentage = 13.64;
            Assert.AreEqual(expectedValue, leftToSpendStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, leftToSpendStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetTotalLeftToSpendStatistics_WhenNothingLeftToSpend_ReturnComputedStatistics() {
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
            savingRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(savingListWithData);

            int totalIncomes = 5500;
            int totalExpenses = 1650;
            int totalDebts = 1100;
            int totalSavings = 2750;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics leftToSpendStatistics = budgetSummaryService.GetTotalLeftToSpendStatistics(totalIncomes, totalExpenses, totalDebts, totalSavings);

            int expectedValue = 0;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, leftToSpendStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, leftToSpendStatistics.TotalPercentage);
        }

        [TestMethod]
        public void GetTotalLeftToSpendStatistics_WhenIncomesEqualZero_ReturnDefaultValues() {
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
            savingRepository.GetByUserIdAndDateInterval(validUserId, validStartDate, validEndDate).Returns(savingListWithData);

            int totalIncomes = 0;
            int totalExpenses = 0;
            int totalDebts = 0;
            int totalSavings = 0;
            BudgetSummaryService budgetSummaryService = new BudgetSummaryService(incomeRepository, expenseRepository, debtRepository, savingRepository, userRepository, userSessionService);
            BudgetItemStatistics leftToSpendStatistics = budgetSummaryService.GetTotalLeftToSpendStatistics(totalIncomes, totalExpenses, totalDebts, totalSavings);

            int expectedValue = 0;
            double expectedPercentage = 0;
            Assert.AreEqual(expectedValue, leftToSpendStatistics.TotalValue);
            Assert.AreEqual(expectedPercentage, leftToSpendStatistics.TotalPercentage);
        }
    }
}
