using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.misc;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.service.query;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerTest.integration.utils;
using MySql.Data.MySqlClient;
using NSubstitute;
using System.Data;
using Testcontainers.MySql;

namespace AdvancedBudgetManagerTest.integration.service {
    [TestClass]
    public class IncomeQueryServiceIntegrationTests : IntegrationTestBase {
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

        private static MySqlContainer mySqlDbContainer;
        private static IncomeQueryService incomeQueryService;
        private static IUserSessionService userSessionService;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            try {
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
                string incomeDataScriptPath = ".\\resources\\scripts\\insert_incomes_data.sql";
                DatabaseSeed.PopulateDb(incomeDataScriptPath, mySqlTestConnection)
                    .GetAwaiter()
                    .GetResult();

                //Note
                //Each test retrieves its own connection from the container because once this is used inside the test class it will automatically disposed inside the using block. Using a shared connection in this case would break the tests.

            } catch (Exception ex) {
                throw new Exception("Class initialization failed", ex);
            }


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
        }

        //[TestMethod]
        public void TestDatabaseCreation() {
            string getAllIncomesQuery = "SELECT * FROM incomes";

            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlCommand getAllIncomesCommand = new MySqlCommand(getAllIncomesQuery, mySqlTestConnection);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getAllIncomesCommand);
            DataTable allIncomes = new DataTable();

            dataAdapter.Fill(allIncomes);

            int count = allIncomes.Rows.Count;

            Assert.AreEqual(36, count);
        }


        [TestMethod]
        public void GetMonthIncomeList_WhenDataFound_DateMustMatchInput() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            IncomeQueryService incomeQueryService = new IncomeQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<IncomeDto> incomesList = incomeQueryService.GetIncomesByUserIdAndDateInterval(singleMonthValidStartDate, singleMonthValidEndDate);

            Assert.IsNotEmpty(incomesList);

            bool hasCorrectIncomesDate = true;
            foreach (IncomeDto income in incomesList) {
                int currentDay = income.Date.Day;
                int currentYear = income.Date.Year;

                int firstDayOfMonth = singleMonthValidStartDate.Day;
                int lastDayOfMonth = singleMonthValidEndDate.Day;
                int year = singleMonthValidStartDate.Year;

                if (currentDay < firstDayOfMonth || currentDay > lastDayOfMonth || currentYear != year) {
                    hasCorrectIncomesDate = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectIncomesDate);
        }



        [TestMethod]
        public void GetMonthIncomeList_WhenNoDataFound_IncomeListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection = new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            IncomeQueryService incomeQueryService = new IncomeQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<IncomeDto> incomesList = incomeQueryService.GetIncomesByUserIdAndDateInterval(singleMonthInvalidStartDate, singleMonthInvalidEndDate);

            Assert.IsEmpty(incomesList);
        }

        [TestMethod]
        public void GetMonthIntervalIncomeList_WhenDataFound_DateMustMatchInput() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            IncomeQueryService incomeQueryService = new IncomeQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<IncomeDto> incomesList = incomeQueryService.GetIncomesByUserIdAndDateInterval(monthIntervalValidStartDate, monthIntervalValidEndDate);

            Assert.IsNotEmpty(incomesList);

            bool hasCorrectIncomesDate = true;
            foreach (IncomeDto income in incomesList) {
                int currentMonth = income.Date.Month;
                int currentYear = income.Date.Year;

                int firstMonthOfInterval = monthIntervalValidStartDate.Month;
                int lastMonthOfInterval = monthIntervalValidEndDate.Month;
                int year = monthIntervalValidStartDate.Year;

                if (currentMonth < firstMonthOfInterval || currentMonth > lastMonthOfInterval || currentYear != year) {
                    hasCorrectIncomesDate = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectIncomesDate);
        }

        [TestMethod]
        public void GetMonthIntervalIncomeList_WhenNoDataFound_IncomeListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            IncomeQueryService incomeQueryService = new IncomeQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<IncomeDto> incomesList = incomeQueryService.GetIncomesByUserIdAndDateInterval(monthIntervalInvalidStartDate, monthIntervalInvalidEndDate);

            Assert.IsEmpty(incomesList);
        }



        //[TestMethod]
        //public void CanStartDbContainer() {
        //    Assert.AreEqual(TestcontainersStates.Running, mySqlDbContainer.State);
        //}

        [ClassCleanup]
        public static async Task Cleanup() {
            if (mySqlDbContainer != null) {
                await mySqlDbContainer.StopAsync();
                await mySqlDbContainer.DisposeAsync();
            }
        }
    }
}
