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
    public class SavingQueryServiceIntegrationTests {
        private static long validUserId = -1;
        private static String validEmailAddress = String.Empty;
        private static DateTime singleMonthValidStartDate = DateTime.Now;
        private static DateTime singleMonthValidEndDate = DateTime.Now;
        private static DateTime monthIntervalValidStartDate = DateTime.Now;
        private static DateTime monthIntervalValidEndDate = DateTime.Now;
        private static DateTime singleMonthInvalidStartDate = DateTime.Now;
        private static DateTime singleMonthInvalidEndDate = DateTime.Now;
        private static DateTime monthIntervalInvalidStartDate = DateTime.Now;
        private static DateTime monthIntervalInvalidEndDate = DateTime.Now;
        private static int singleMonthIncomesTotalValue = 0;
        private static int singleMonthSavingsTotalValue = 0;
        private static int multipleMonthsIncomesTotalValue = 0;
        private static int multipleMonthsSavingsTotalValue = 0;
        private static int validMonthlySavingEvolutionYear = 0;
        private static int invalidMonthlySavingEvolutionYear = 0;
        private static int januaryMonthlyTotalSavings = 0;
        private static int februaryMonthlyTotalSavings = 0;
        private static int marchMonthlyTotalSavings = 0;
        private static int aprilMonthlyTotalSavings = 0;
        private static int mayMonthlyTotalSavings = 0;
        private static int juneMonthlyTotalSavings = 0;
        private static int julyMonthlyTotalSavings = 0;
        private static int augustMonthlyTotalSavings = 0;
        private static int septemberMonthlyTotalSavings = 0;
        private static int octoberMonthlyTotalSavings = 0;
        private static int novemberMonthlyTotalSavings = 0;
        private static int decemberMonthlyTotalSavings = 0;
        private static double incomesPercentage = 0.0;
        private static double singleMonthSavingsPercentageFromTotalIncomes = 0.0;
        private static double multipleMonthsSavingsPercentageFromTotalIncomes = 0.0;

        private static MySqlContainer mySqlDbContainer = null!;

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
            string savingDataScriptPath = ".\\resources\\scripts\\insert_savings_data.sql";
            DatabaseSeed.PopulateDb(savingDataScriptPath, mySqlTestConnection)
                .GetAwaiter()
                .GetResult();

            //Note
            //Each test retrieves its own connection from the container because once this is used inside the test class it will automatically be disposed inside the using block. Using a shared connection in this case would break the tests.

            validUserId = Convert.ToInt32(testContext.Properties["validUserId"]?.ToString() ?? String.Empty);
            //invalidUserId = Convert.ToInt32(testContext.Properties["invalidUserId"]?.ToString() ?? String.Empty);
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;
            DateTime.TryParse(testContext.Properties["singleMonthValidStartDate"]?.ToString() ?? String.Empty, out singleMonthValidStartDate);
            DateTime.TryParse(testContext.Properties["singleMonthValidEndDate"]?.ToString() ?? String.Empty, out singleMonthValidEndDate);
            DateTime.TryParse(testContext.Properties["monthIntervalValidStartDate"]?.ToString() ?? String.Empty, out monthIntervalValidStartDate);
            DateTime.TryParse(testContext.Properties["monthIntervalValidEndDate"]?.ToString() ?? String.Empty, out monthIntervalValidEndDate);
            DateTime.TryParse(testContext.Properties["singleMonthInvalidStartDate"]?.ToString() ?? String.Empty, out singleMonthInvalidStartDate);
            DateTime.TryParse(testContext.Properties["singleMonthInvalidEndDate"]?.ToString() ?? String.Empty, out singleMonthInvalidEndDate);
            DateTime.TryParse(testContext.Properties["monthIntervalInvalidStartDate"]?.ToString() ?? String.Empty, out monthIntervalInvalidStartDate);
            DateTime.TryParse(testContext.Properties["monthIntervalInvalidEndDate"]?.ToString() ?? String.Empty, out monthIntervalInvalidEndDate);
            singleMonthIncomesTotalValue = Convert.ToInt32(testContext.Properties["singleMonthIncomesTotalValue"]?.ToString());
            singleMonthSavingsTotalValue = Convert.ToInt32(testContext.Properties["singleMonthSavingsTotalValue"]?.ToString());
            multipleMonthsIncomesTotalValue = Convert.ToInt32(testContext.Properties["multipleMonthsIncomesTotalValue"]?.ToString());
            multipleMonthsSavingsTotalValue = Convert.ToInt32(testContext.Properties["multipleMonthsSavingsTotalValue"]?.ToString());
            validMonthlySavingEvolutionYear = Convert.ToInt32(testContext.Properties["validMonthlyIncomeEvolutionYear"]?.ToString() ?? String.Empty);
            invalidMonthlySavingEvolutionYear = Convert.ToInt32(testContext.Properties["invalidMonthlyIncomeEvolutionYear"]?.ToString() ?? String.Empty);
            januaryMonthlyTotalSavings = Convert.ToInt32(testContext.Properties
            ["januaryMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            februaryMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["februaryMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            marchMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["marchMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            aprilMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["aprilMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            mayMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["mayMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            juneMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["juneMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            julyMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["julyMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            augustMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["augustMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            septemberMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["septemberMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            octoberMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["octoberMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            novemberMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["novemberMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            decemberMonthlyTotalSavings = Convert.ToInt32(testContext.Properties["decemberMonthlyTotalSavings"]?.ToString() ?? String.Empty);
            incomesPercentage = Convert.ToDouble(testContext.Properties["incomesPercentage"]?.ToString());
            singleMonthSavingsPercentageFromTotalIncomes = Convert.ToDouble(testContext.Properties["singleMonthSavingsPercentageFromTotalIncomes"]?.ToString());
            multipleMonthsSavingsPercentageFromTotalIncomes = Convert.ToDouble(testContext.Properties["multipleMonthsSavingsPercentageFromTotalIncomes"]?.ToString());
        }

        [TestMethod]
        public void GetSingleMonthSavingList_WhenDataFound_DateMatchesInput() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<SavingDto> savingList = savingQueryService.GetSavingsByUserIdAndDateInterval(singleMonthValidStartDate, singleMonthValidEndDate);

            Assert.IsNotEmpty(savingList);

            bool hasCorrectSavingsDate = true;
            foreach (SavingDto saving in savingList) {
                int currentDay = saving.Date.Day;
                int currentYear = saving.Date.Year;

                int firstDayOfMonth = singleMonthValidStartDate.Day;
                int lastDayOfMonth = singleMonthValidEndDate.Day;
                int year = singleMonthValidStartDate.Year;

                if (currentDay < firstDayOfMonth || currentDay > lastDayOfMonth || currentYear != year) {
                    hasCorrectSavingsDate = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectSavingsDate);
        }

        [TestMethod]
        public void GetSingleMonthSavingList_WhenNoDataFound_SavingListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection = new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<SavingDto> savingsList = savingQueryService.GetSavingsByUserIdAndDateInterval(singleMonthInvalidStartDate, singleMonthInvalidEndDate);

            Assert.IsEmpty(savingsList);
        }

        [TestMethod]
        public void GetMonthIntervalSavingList_WhenDataFound_DateMatchesInput() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<SavingDto> savingsList = savingQueryService.GetSavingsByUserIdAndDateInterval(monthIntervalValidStartDate, monthIntervalValidEndDate);

            Assert.IsNotEmpty(savingsList);

            bool hasCorrectSavingsDate = true;
            foreach (SavingDto saving in savingsList) {
                int currentMonth = saving.Date.Month;
                int currentYear = saving.Date.Year;

                int firstMonthOfInterval = monthIntervalValidStartDate.Month;
                int lastMonthOfInterval = monthIntervalValidEndDate.Month;
                int year = monthIntervalValidStartDate.Year;

                if (currentMonth < firstMonthOfInterval || currentMonth > lastMonthOfInterval || currentYear != year) {
                    hasCorrectSavingsDate = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectSavingsDate);
        }

        [TestMethod]
        public void GetMonthIntervalSavingList_WhenNoDataFound_SavingListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<SavingDto> savingsList = savingQueryService.GetSavingsByUserIdAndDateInterval(monthIntervalInvalidStartDate, monthIntervalInvalidEndDate);

            Assert.IsEmpty(savingsList);
        }

        [TestMethod]
        public void GetSingleMonthSavingStatistics_WhenDataFound_StatisticsDataMatches() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<BudgetItemStatistics> savingsPercentageStatistics = savingQueryService.GetSavingsPercentageFromTotalIncomes(singleMonthValidStartDate, singleMonthValidEndDate);

            foreach (BudgetItemStatistics budgetItemStatistic in savingsPercentageStatistics) {
                switch (budgetItemStatistic.BudgetItem) {
                    case BudgetItem.Income:
                        Assert.AreEqual(incomesPercentage, budgetItemStatistic.TotalPercentage);
                        Assert.AreEqual(singleMonthIncomesTotalValue, budgetItemStatistic.TotalValue);
                        break;

                    case BudgetItem.Saving:
                        Assert.AreEqual(singleMonthSavingsPercentageFromTotalIncomes, budgetItemStatistic.TotalPercentage);
                        Assert.AreEqual(singleMonthSavingsTotalValue, budgetItemStatistic.TotalValue);
                        break;

                    default:
                        Assert.Fail($"Unknown budget item found:{budgetItemStatistic.BudgetItem.ToString()}");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetSingleMonthSavingStatistics_WhenNoDataFound_StatisticsListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<BudgetItemStatistics> savingsPercentageStatistics = savingQueryService.GetSavingsPercentageFromTotalIncomes(singleMonthInvalidStartDate, singleMonthInvalidEndDate);

            Assert.IsEmpty(savingsPercentageStatistics);
        }

        [TestMethod]
        public void GetMonthIntervalSavingStatistics_WhenDataFound_StatisticsDataMatches() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<BudgetItemStatistics> savingsPercentageStatistics = savingQueryService.GetSavingsPercentageFromTotalIncomes(monthIntervalValidStartDate, monthIntervalValidEndDate);

            foreach (BudgetItemStatistics budgetItemStatistic in savingsPercentageStatistics) {
                switch (budgetItemStatistic.BudgetItem) {
                    case BudgetItem.Income:
                        Assert.AreEqual(incomesPercentage, budgetItemStatistic.TotalPercentage);
                        Assert.AreEqual(multipleMonthsIncomesTotalValue, budgetItemStatistic.TotalValue);
                        break;

                    case BudgetItem.Saving:
                        Assert.AreEqual(multipleMonthsSavingsPercentageFromTotalIncomes, budgetItemStatistic.TotalPercentage);
                        Assert.AreEqual(multipleMonthsSavingsTotalValue, budgetItemStatistic.TotalValue);
                        break;

                    default:
                        Assert.Fail($"Unknown budget item found:{budgetItemStatistic.BudgetItem.ToString()}");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetMonthIntervalSavingStatistics_WhenNoDataFound_StatisticsListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<BudgetItemStatistics> savingsPercentageStatistics = savingQueryService.GetSavingsPercentageFromTotalIncomes(monthIntervalInvalidStartDate, monthIntervalInvalidEndDate);

            Assert.IsEmpty(savingsPercentageStatistics);
        }

        [TestMethod]
        public void GetMonthlySavingEvolution_WhenDataFound_MonthlySavingValuesMatch() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemMonthlyEvolutionDto monthlySavingEvolutionDto = savingQueryService.GetMonthlySavingsEvolution(validMonthlySavingEvolutionYear);

            Dictionary<Month, int> monthlySavingStatistics = monthlySavingEvolutionDto.MonthlyStatistics;
            foreach (KeyValuePair<Month, int> monthlySavings in monthlySavingStatistics.ToList()) {
                switch (monthlySavings.Key) {
                    case Month.January:
                        Assert.AreEqual(januaryMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.February:
                        Assert.AreEqual(februaryMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.March:
                        Assert.AreEqual(marchMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.April:
                        Assert.AreEqual(aprilMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.May:
                        Assert.AreEqual(mayMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.June:
                        Assert.AreEqual(juneMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.July:
                        Assert.AreEqual(julyMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.August:
                        Assert.AreEqual(augustMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.September:
                        Assert.AreEqual(septemberMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.October:
                        Assert.AreEqual(octoberMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.November:
                        Assert.AreEqual(novemberMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    case Month.December:
                        Assert.AreEqual(decemberMonthlyTotalSavings, monthlySavings.Value);
                        break;

                    default:
                        Assert.Fail("Invalid month value retrieved from the response object.");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetMonthlySavingEvolution_WhenNoDataFound_MonthlySavingStatisticsIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            SavingQueryService savingQueryService = new SavingQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemMonthlyEvolutionDto monthlySavingEvolutionDto = savingQueryService.GetMonthlySavingsEvolution(invalidMonthlySavingEvolutionYear);

            Dictionary<Month, int> monthlySavingStatistics = monthlySavingEvolutionDto.MonthlyStatistics;

            Assert.IsEmpty(monthlySavingStatistics);
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
