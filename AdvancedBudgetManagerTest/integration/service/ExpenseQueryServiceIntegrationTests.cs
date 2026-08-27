using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.service.query;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerTest.integration.utils;
using MySql.Data.MySqlClient;
using NSubstitute;
using Testcontainers.MySql;

namespace AdvancedBudgetManagerTest.integration.service {
    [TestClass]
    public class ExpenseQueryServiceIntegrationTests {
        private static long validUserId = -1;
        private static long invalidUserId = -1;
        private static String validEmailAddress = String.Empty;
        private static DateTime singleMonthValidStartDate = DateTime.Now;
        private static DateTime singleMonthValidEndDate = DateTime.Now;
        private static DateTime monthIntervalValidStartDate = DateTime.Now;
        private static DateTime monthIntervalValidEndDate = DateTime.Now;
        private static DateTime singleMonthInvalidStartDate = DateTime.Now;
        private static DateTime singleMonthInvalidEndDate = DateTime.Now;
        private static DateTime monthIntervalInvalidStartDate = DateTime.Now;
        private static DateTime monthIntervalInvalidEndDate = DateTime.Now;
        private static int validMonthlyExpenseEvolutionYear = 0;
        private static int invalidMonthlyExpenseEvolutionYear = 0;
        private static double singleMonthClothingPercentage = 0;
        private static int singleMonthClothingValue = 0;
        private static double singleMonthEntertainmentPercentage = 0;
        private static int singleMonthEntertainmentValue = 0;
        private static double singleMonthFoodPercentage = 0;
        private static int singleMonthFoodValue = 0;
        private static double singleMonthHealthcarePercentage = 0;
        private static int singleMonthHealthcareValue = 0;
        private static double singleMonthITCPercentage = 0;
        private static int singleMonthITCValue = 0;
        private static double singleMonthRestaurantsPercentage = 0;
        private static int singleMonthRestaurantsValue = 0;
        private static double singleMonthSportPercentage = 0;
        private static int singleMonthSportValue = 0;
        private static double singleMonthTransportPercentage = 0;
        private static int singleMonthTransportValue = 0;
        private static double singleMonthUtilitiesPercentage = 0;
        private static int singleMonthUtilitiesValue = 0;
        private static double monthIntervalClothingPercentage = 0;
        private static int monthIntervalClothingValue = 0;
        private static double monthIntervalEducationPercentage = 0;
        private static int monthIntervalEducationValue = 0;
        private static double monthIntervalEntertainmentPercentage = 0;
        private static int monthIntervalEntertainmentValue = 0;
        private static double monthIntervalFoodPercentage = 0;
        private static int monthIntervalFoodValue = 0;
        private static double monthIntervalHealthcarePercentage = 0;
        private static int monthIntervalHealthcareValue = 0;
        private static double monthIntervalITCPercentage = 0;
        private static int monthIntervalITCValue = 0;
        private static double monthIntervalRestaurantsPercentage = 0;
        private static int monthIntervalRestaurantsValue = 0;
        private static double monthIntervalSportPercentage = 0;
        private static int monthIntervalSportValue = 0;
        private static double monthIntervalTransportPercentage = 0;
        private static int monthIntervalTransportValue = 0;
        private static double monthIntervalUtilitiesPercentage = 0;
        private static int monthIntervalUtilitiesValue = 0;
        private static int januaryMonthlyTotalExpenses = 0;
        private static int februaryMonthlyTotalExpenses = 0;
        private static int marchMonthlyTotalExpenses = 0;
        private static int aprilMonthlyTotalExpenses = 0;
        private static int mayMonthlyTotalExpenses = 0;
        private static int juneMonthlyTotalExpenses = 0;
        private static int julyMonthlyTotalExpenses = 0;
        private static int augustMonthlyTotalExpenses = 0;
        private static int septemberMonthlyTotalExpenses = 0;
        private static int octoberMonthlyTotalExpenses = 0;
        private static int novemberMonthlyTotalExpenses = 0;
        private static int decemberMonthlyTotalExpenses = 0;


        private static MySqlContainer mySqlDbContainer;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            //Initializes the database container
            IntegrationTestBase testBase = new IntegrationTestBase();
            testBase.Initialize().GetAwaiter().GetResult();

            //Retrieves the database container after initialization
            mySqlDbContainer = testBase.MySqlDbContainer;

            //Retrieves the database connection of the container for setting up the database
            MySqlConnection mySqlTestConnection =
                new MySqlConnection(mySqlDbContainer.GetConnectionString());
            mySqlTestConnection.Open();

            //Creates the database tables
            DatabaseSchema.CreateAsync(mySqlTestConnection)
                .GetAwaiter()
                .GetResult();

            //Populates the database with data using the specified script file
            string incomeDataScriptPath = ".\\resources\\scripts\\insert_expenses_data.sql";
            DatabaseSeed.PopulateDb(incomeDataScriptPath, mySqlTestConnection)
                .GetAwaiter()
                .GetResult();

            //Note
            //Each test retrieves its own connection from the container because once this is used inside the test class it will automatically disposed inside the using block. Using a shared connection in this case would break the tests.

            validUserId = Convert.ToInt32(testContext.Properties["validUserId"]?.ToString() ?? String.Empty);
            invalidUserId = Convert.ToInt32(testContext.Properties["invalidUserId"]?.ToString() ?? String.Empty);
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;
            DateTime.TryParse(testContext.Properties["singleMonthValidStartDate"]?.ToString() ?? String.Empty, out singleMonthValidStartDate);
            DateTime.TryParse(testContext.Properties["singleMonthValidEndDate"]?.ToString() ?? String.Empty, out singleMonthValidEndDate);
            DateTime.TryParse(testContext.Properties["monthIntervalValidStartDate"]?.ToString() ?? String.Empty, out monthIntervalValidStartDate);
            DateTime.TryParse(testContext.Properties["monthIntervalValidEndDate"]?.ToString() ?? String.Empty, out monthIntervalValidEndDate);
            DateTime.TryParse(testContext.Properties["singleMonthInvalidStartDate"]?.ToString() ?? String.Empty, out singleMonthInvalidStartDate);
            DateTime.TryParse(testContext.Properties["singleMonthInvalidEndDate"]?.ToString() ?? String.Empty, out singleMonthInvalidEndDate);
            DateTime.TryParse(testContext.Properties["monthIntervalInvalidStartDate"]?.ToString() ?? String.Empty, out monthIntervalInvalidStartDate);
            DateTime.TryParse(testContext.Properties["monthIntervalInvalidEndDate"]?.ToString() ?? String.Empty, out monthIntervalInvalidEndDate);
            validMonthlyExpenseEvolutionYear = Convert.ToInt32(testContext.Properties["validMonthlyIncomeEvolutionYear"]?.ToString() ?? String.Empty);
            invalidMonthlyExpenseEvolutionYear = Convert.ToInt32(testContext.Properties["invalidMonthlyIncomeEvolutionYear"]?.ToString() ?? String.Empty);
            singleMonthClothingPercentage = Convert.ToDouble(testContext.Properties["singleMonthClothingPercentage"]?.ToString() ?? String.Empty);
            singleMonthClothingValue = Convert.ToInt32(testContext.Properties["singleMonthClothingValue"]?.ToString() ?? String.Empty);
            singleMonthEntertainmentPercentage = Convert.ToDouble(testContext.Properties["singleMonthEntertainmentPercentage"]?.ToString() ?? String.Empty);
            singleMonthEntertainmentValue = Convert.ToInt32(testContext.Properties["singleMonthEntertainmentValue"]?.ToString() ?? String.Empty);
            singleMonthFoodPercentage = Convert.ToDouble(testContext.Properties["singleMonthFoodPercentage"]?.ToString() ?? String.Empty);
            singleMonthFoodValue = Convert.ToInt32(testContext.Properties["singleMonthFoodValue"]?.ToString() ?? String.Empty);
            singleMonthHealthcarePercentage = Convert.ToDouble(testContext.Properties["singleMonthHealthcarePercentage"]?.ToString() ?? String.Empty);
            singleMonthHealthcareValue = Convert.ToInt32(testContext.Properties["singleMonthHealthcareValue"]?.ToString() ?? String.Empty);
            singleMonthITCPercentage = Convert.ToDouble(testContext.Properties["singleMonthITCPercentage"]?.ToString() ?? String.Empty);
            singleMonthITCValue = Convert.ToInt32(testContext.Properties["singleMonthITCValue"]?.ToString() ?? String.Empty);
            singleMonthRestaurantsPercentage = Convert.ToDouble(testContext.Properties["singleMonthRestaurantsPercentage"]?.ToString() ?? String.Empty);
            singleMonthRestaurantsValue = Convert.ToInt32(testContext.Properties["singleMonthRestaurantsValue"]?.ToString() ?? String.Empty);
            singleMonthSportPercentage = Convert.ToDouble(testContext.Properties["singleMonthSportPercentage"]?.ToString() ?? String.Empty);
            singleMonthSportValue = Convert.ToInt32(testContext.Properties["singleMonthSportValue"]?.ToString() ?? String.Empty);
            singleMonthTransportPercentage = Convert.ToDouble(testContext.Properties["singleMonthTransportPercentage"]?.ToString() ?? String.Empty);
            singleMonthTransportValue = Convert.ToInt32(testContext.Properties["singleMonthTransportValue"]?.ToString() ?? String.Empty);
            singleMonthUtilitiesPercentage = Convert.ToDouble(testContext.Properties["singleMonthUtilitiesPercentage"]?.ToString() ?? String.Empty);
            singleMonthUtilitiesValue = Convert.ToInt32(testContext.Properties["singleMonthUtilitiesValue"]?.ToString() ?? String.Empty);
            monthIntervalClothingPercentage = Convert.ToDouble(testContext.Properties["monthIntervalClothingPercentage"]?.ToString() ?? String.Empty);
            monthIntervalClothingValue = Convert.ToInt32(testContext.Properties["monthIntervalClothingValue"]?.ToString() ?? String.Empty);
            monthIntervalEducationPercentage = Convert.ToDouble(testContext.Properties["monthIntervalEducationPercentage"]?.ToString() ?? String.Empty);
            monthIntervalEducationValue = Convert.ToInt32(testContext.Properties["monthIntervalEducationValue"]?.ToString() ?? String.Empty);
            monthIntervalEntertainmentPercentage = Convert.ToDouble(testContext.Properties["monthIntervalEntertainmentPercentage"]?.ToString() ?? String.Empty);
            monthIntervalEntertainmentValue = Convert.ToInt32(testContext.Properties["monthIntervalEntertainmentValue"]?.ToString() ?? String.Empty);
            monthIntervalFoodPercentage = Convert.ToDouble(testContext.Properties["monthIntervalFoodPercentage"]?.ToString() ?? String.Empty);
            monthIntervalFoodValue = Convert.ToInt32(testContext.Properties["monthIntervalFoodValue"]?.ToString() ?? String.Empty);
            monthIntervalHealthcarePercentage = Convert.ToDouble(testContext.Properties["monthIntervalHealthcarePercentage"]?.ToString() ?? String.Empty);
            monthIntervalHealthcareValue = Convert.ToInt32(testContext.Properties["monthIntervalHealthcareValue"]?.ToString() ?? String.Empty);
            monthIntervalITCPercentage = Convert.ToDouble(testContext.Properties["monthIntervalITCPercentage"]?.ToString() ?? String.Empty);
            monthIntervalITCValue = Convert.ToInt32(testContext.Properties["monthIntervalITCValue"]?.ToString() ?? String.Empty);
            monthIntervalRestaurantsPercentage = Convert.ToDouble(testContext.Properties["monthIntervalRestaurantsPercentage"]?.ToString() ?? String.Empty);
            monthIntervalRestaurantsValue = Convert.ToInt32(testContext.Properties["monthIntervalRestaurantsValue"]?.ToString() ?? String.Empty);
            monthIntervalSportPercentage = Convert.ToDouble(testContext.Properties["monthIntervalSportPercentage"]?.ToString() ?? String.Empty);
            monthIntervalSportValue = Convert.ToInt32(testContext.Properties["monthIntervalSportValue"]?.ToString() ?? String.Empty);
            monthIntervalTransportPercentage = Convert.ToDouble(testContext.Properties["monthIntervalTransportPercentage"]?.ToString() ?? String.Empty);
            monthIntervalTransportValue = Convert.ToInt32(testContext.Properties["monthIntervalTransportValue"]?.ToString() ?? String.Empty);
            monthIntervalUtilitiesPercentage = Convert.ToDouble(testContext.Properties["monthIntervalUtilitiesPercentage"]?.ToString() ?? String.Empty);
            monthIntervalUtilitiesValue = Convert.ToInt32(testContext.Properties["monthIntervalUtilitiesValue"]?.ToString() ?? String.Empty);
            januaryMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["januaryMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            februaryMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["februaryMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            marchMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["marchMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            aprilMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["aprilMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            mayMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["mayMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            juneMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["juneMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            julyMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["julyMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            augustMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["augustMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            septemberMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["septemberMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            octoberMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["octoberMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            novemberMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["novemberMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
            decemberMonthlyTotalExpenses = Convert.ToInt32(testContext.Properties["decemberMonthlyTotalExpenses"]?.ToString() ?? String.Empty);
        }

        [TestMethod]
        public void GetSingleMonthExpenseList_WhenDataFound_DateMatchesInput() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<ExpenseDto> expenseList = expenseQueryService.GetExpensesByUserIdAndDateInterval(singleMonthValidStartDate, singleMonthValidEndDate);

            Assert.IsNotEmpty(expenseList);

            bool hasCorrectExpensesDate = true;
            foreach (ExpenseDto expense in expenseList) {
                int currentDay = expense.Date.Day;
                int currentYear = expense.Date.Year;

                int firstDayOfMonth = singleMonthValidStartDate.Day;
                int lastDayOfMonth = singleMonthValidEndDate.Day;
                int year = singleMonthValidStartDate.Year;

                if (currentDay < firstDayOfMonth || currentDay > lastDayOfMonth || currentYear != year) {
                    hasCorrectExpensesDate = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectExpensesDate);
        }



        [TestMethod]
        public void GetSingleMonthExpenseList_WhenNoDataFound_ExpenseListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection = new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<ExpenseDto> expenseList = expenseQueryService.GetExpensesByUserIdAndDateInterval(singleMonthInvalidStartDate, singleMonthInvalidEndDate);

            Assert.IsEmpty(expenseList);
        }

        [TestMethod]
        public void GetMonthIntervalExpenseList_WhenDataFound_DateMatchesInput() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<ExpenseDto> incomesList = expenseQueryService.GetExpensesByUserIdAndDateInterval(monthIntervalValidStartDate, monthIntervalValidEndDate);

            Assert.IsNotEmpty(incomesList);

            bool hasCorrectExpensesDate = true;
            foreach (ExpenseDto expense in incomesList) {
                int currentMonth = expense.Date.Month;
                int currentYear = expense.Date.Year;

                int firstMonthOfInterval = monthIntervalValidStartDate.Month;
                int lastMonthOfInterval = monthIntervalValidEndDate.Month;
                int year = monthIntervalValidStartDate.Year;

                if (currentMonth < firstMonthOfInterval || currentMonth > lastMonthOfInterval || currentYear != year) {
                    hasCorrectExpensesDate = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectExpensesDate);
        }

        [TestMethod]
        public void GetMonthIntervalExpenseList_WhenNoDataFound_ExpenseListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<ExpenseDto> expensesList = expenseQueryService.GetExpensesByUserIdAndDateInterval(monthIntervalInvalidStartDate, monthIntervalInvalidEndDate);

            Assert.IsEmpty(expensesList);
        }

        [TestMethod]
        public void GetSingleMonthExpenseStatistics_WhenDataFound_StatisticsDataMatches() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemCategoriesStatisticsDto expenseCategoriesStatistics = expenseQueryService.GetAggregatedExpensesByCategory(singleMonthValidStartDate, singleMonthValidEndDate);

            foreach (CategoryStatisticsDto categoryStatisticsDto in expenseCategoriesStatistics.CategoriesStatistics) {
                switch (categoryStatisticsDto.Name) {
                    case "Clothing":
                        Assert.AreEqual(singleMonthClothingPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthClothingValue, categoryStatisticsDto.Value);
                        break;

                    case "Entertainment":
                        Assert.AreEqual(singleMonthEntertainmentPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthEntertainmentValue, categoryStatisticsDto.Value);
                        break;

                    case "Food":
                        Assert.AreEqual(singleMonthFoodPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthFoodValue, categoryStatisticsDto.Value);
                        break;

                    case "Healthcare":
                        Assert.AreEqual(singleMonthHealthcarePercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthHealthcareValue, categoryStatisticsDto.Value);
                        break;

                    case "IT&C":
                        Assert.AreEqual(singleMonthITCPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthITCValue, categoryStatisticsDto.Value);
                        break;

                    case "Restaurants/bars/cafes":
                        Assert.AreEqual(singleMonthRestaurantsPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthRestaurantsValue, categoryStatisticsDto.Value);
                        break;

                    case "Sport":
                        Assert.AreEqual(singleMonthSportPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthSportValue, categoryStatisticsDto.Value);
                        break;

                    case "Transport":
                        Assert.AreEqual(singleMonthTransportPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthTransportValue, categoryStatisticsDto.Value);
                        break;

                    case "Utilities":
                        Assert.AreEqual(singleMonthUtilitiesPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(singleMonthUtilitiesValue, categoryStatisticsDto.Value);
                        break;

                    default:
                        Assert.Fail($"Unknown expense category found:{categoryStatisticsDto.Name}");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetSingleMonthExpenseStatistics_WhenNoDataFound_CategoriesListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemCategoriesStatisticsDto expenseCategoriesStatistics = expenseQueryService.GetAggregatedExpensesByCategory(singleMonthInvalidStartDate, singleMonthInvalidEndDate);

            List<CategoryStatisticsDto> expenseCategoriesStatisticsList = expenseCategoriesStatistics.CategoriesStatistics;

            Assert.IsEmpty(expenseCategoriesStatisticsList);
        }

        [TestMethod]
        public void GetMonthIntervalExpenseStatistics_WhenDataFound_StatisticsDataMatches() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemCategoriesStatisticsDto expenseCategoriesStatistics = expenseQueryService.GetAggregatedExpensesByCategory(monthIntervalValidStartDate, monthIntervalValidEndDate);

            foreach (CategoryStatisticsDto categoryStatisticsDto in expenseCategoriesStatistics.CategoriesStatistics) {
                switch (categoryStatisticsDto.Name) {
                    case "Clothing":
                        Assert.AreEqual(monthIntervalClothingPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalClothingValue, categoryStatisticsDto.Value);
                        break;

                    case "Education":
                        Assert.AreEqual(monthIntervalEducationPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalEducationValue, categoryStatisticsDto.Value);
                        break;

                    case "Entertainment":
                        Assert.AreEqual(monthIntervalEntertainmentPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalEntertainmentValue, categoryStatisticsDto.Value);
                        break;

                    case "Food":
                        Assert.AreEqual(monthIntervalFoodPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalFoodValue, categoryStatisticsDto.Value);
                        break;

                    case "Healthcare":
                        Assert.AreEqual(monthIntervalHealthcarePercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalHealthcareValue, categoryStatisticsDto.Value);
                        break;

                    case "IT&C":
                        Assert.AreEqual(monthIntervalITCPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalITCValue, categoryStatisticsDto.Value);
                        break;

                    case "Restaurants/bars/cafes":
                        Assert.AreEqual(monthIntervalRestaurantsPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalRestaurantsValue, categoryStatisticsDto.Value);
                        break;

                    case "Sport":
                        Assert.AreEqual(monthIntervalSportPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalSportValue, categoryStatisticsDto.Value);
                        break;

                    case "Transport":
                        Assert.AreEqual(monthIntervalTransportPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalTransportValue, categoryStatisticsDto.Value);
                        break;

                    case "Utilities":
                        Assert.AreEqual(monthIntervalUtilitiesPercentage, categoryStatisticsDto.Percentage);
                        Assert.AreEqual(monthIntervalUtilitiesValue, categoryStatisticsDto.Value);
                        break;

                    default:
                        Assert.Fail($"Unknown expense category found:{categoryStatisticsDto.Name}");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetMonthIntervalExpenseStatistics_WhenNoDataFound_CategoriesListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemCategoriesStatisticsDto expenseCategoriesStatistics = expenseQueryService.GetAggregatedExpensesByCategory(monthIntervalInvalidStartDate, monthIntervalInvalidEndDate);

            List<CategoryStatisticsDto> expenseCategoriesStatisticsList = expenseCategoriesStatistics.CategoriesStatistics;

            Assert.IsEmpty(expenseCategoriesStatisticsList);
        }

        [TestMethod]
        public void GetMonthlyExpenseEvolution_WhenDataFound_MonthlyExpenseValuesMatch() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemMonthlyEvolutionDto monthlyExpenseEvolutionDto = expenseQueryService.GetMonthlyExpensesEvolution(validMonthlyExpenseEvolutionYear);

            Dictionary<Month, int> monthlyExpenseStatistics = monthlyExpenseEvolutionDto.MonthlyStatistics;
            foreach (KeyValuePair<Month, int> monthlyExpenses in monthlyExpenseStatistics.ToList()) {
                switch (monthlyExpenses.Key) {
                    case Month.January:
                        Assert.AreEqual(januaryMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.February:
                        Assert.AreEqual(februaryMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.March:
                        Assert.AreEqual(marchMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.April:
                        Assert.AreEqual(aprilMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.May:
                        Assert.AreEqual(mayMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.June:
                        Assert.AreEqual(juneMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.July:
                        Assert.AreEqual(julyMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.August:
                        Assert.AreEqual(augustMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.September:
                        Assert.AreEqual(septemberMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.October:
                        Assert.AreEqual(octoberMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.November:
                        Assert.AreEqual(novemberMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    case Month.December:
                        Assert.AreEqual(decemberMonthlyTotalExpenses, monthlyExpenses.Value);
                        break;

                    default:
                        Assert.Fail("Invalid month value retrieved from the response object.");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetMonthlyExpenseEvolution_WhenNoDataFound_MonthlyExpenseStatisticsIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            ExpenseQueryService expenseQueryService = new ExpenseQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemMonthlyEvolutionDto monthlyExpenseEvolutionDto = expenseQueryService.GetMonthlyExpensesEvolution(invalidMonthlyExpenseEvolutionYear);

            Dictionary<Month, int> monthlyExpenseStatistics = monthlyExpenseEvolutionDto.MonthlyStatistics;

            Assert.IsEmpty(monthlyExpenseStatistics);
        }

        [ClassCleanup]
        public static async Task Cleanup() {
            if (mySqlDbContainer != null) {
                await mySqlDbContainer.StopAsync();
                await mySqlDbContainer.DisposeAsync();
            }
        }
    }
}
