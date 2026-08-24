using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.service.query;
using AdvancedBudgetManagerCore.utils.database;
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
        private static int validMonthlyIncomeEvolutionYear = 0;
        private static int invalidMonthlyIncomeEvolutionYear = 0;
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
            validMonthlyIncomeEvolutionYear = Convert.ToInt32(testContext.Properties["validMonthlyIncomeEvolutionYear"]?.ToString() ?? String.Empty);
            invalidMonthlyIncomeEvolutionYear = Convert.ToInt32(testContext.Properties["invalidMonthlyIncomeEvolutionYear"]?.ToString() ?? String.Empty);
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
    }
}
