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
    public class DebtQueryServiceIntegrationTests {
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
        private static int validMonthlyDebtEvolutionYear = 0;
        private static int invalidMonthlyDebtEvolutionYear = 0;
        private static double singleMonthDebtPercentageOwedToFred = 0;
        private static int singleMonthDebtValueOwedToFred = 0;
        private static double singleMonthDebtPercentageOwedToJonathan = 0;
        private static int singleMonthDebtValueOwedToJonathan = 0;
        private static double multipleMonthsDebtPercentageOwedToAndrew = 0;
        private static int multipleMonthsDebtValueOwedToAndrew = 0;
        private static double multipleMonthsDebtPercentageOwedToFred = 0;
        private static int multipleMonthsDebtValueOwedToFred = 0;
        private static double multipleMonthsDebtPercentageOwedToJonathan = 0;
        private static int multipleMonthsDebtValueOwedToJonathan = 0;
        private static int januaryMonthlyTotalDebts = 0;
        private static int februaryMonthlyTotalDebts = 0;
        private static int marchMonthlyTotalDebts = 0;
        private static int aprilMonthlyTotalDebts = 0;
        private static int mayMonthlyTotalDebts = 0;
        private static int juneMonthlyTotalDebts = 0;
        private static int julyMonthlyTotalDebts = 0;
        private static int augustMonthlyTotalDebts = 0;
        private static int septemberMonthlyTotalDebts = 0;
        private static int octoberMonthlyTotalDebts = 0;
        private static int novemberMonthlyTotalDebts = 0;
        private static int decemberMonthlyTotalDebts = 0;


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
            string incomeDataScriptPath = ".\\resources\\scripts\\insert_debts_data.sql";
            DatabaseSeed.PopulateDb(incomeDataScriptPath, mySqlTestConnection)
                .GetAwaiter()
                .GetResult();

            //Note
            //Each test retrieves its own connection from the container because once this is used inside the test class it will automatically be disposed inside the using block. Using a shared connection in this case would break the tests.

            validUserId = Convert.ToInt32(testContext.Properties["validUserId"]?.ToString() ?? String.Empty);
            validEmailAddress = testContext.Properties["validEmailAddress"]?.ToString() ?? String.Empty;
            DateTime.TryParse(testContext.Properties["singleMonthValidStartDate"]?.ToString() ?? String.Empty, out singleMonthValidStartDate);
            DateTime.TryParse(testContext.Properties["singleMonthValidEndDate"]?.ToString() ?? String.Empty, out singleMonthValidEndDate);
            DateTime.TryParse(testContext.Properties["monthIntervalValidStartDate"]?.ToString() ?? String.Empty, out monthIntervalValidStartDate);
            DateTime.TryParse(testContext.Properties["monthIntervalValidEndDate"]?.ToString() ?? String.Empty, out monthIntervalValidEndDate);
            DateTime.TryParse(testContext.Properties["singleMonthInvalidStartDate"]?.ToString() ?? String.Empty, out singleMonthInvalidStartDate);
            DateTime.TryParse(testContext.Properties["singleMonthInvalidEndDate"]?.ToString() ?? String.Empty, out singleMonthInvalidEndDate);
            DateTime.TryParse(testContext.Properties["monthIntervalInvalidStartDate"]?.ToString() ?? String.Empty, out monthIntervalInvalidStartDate);
            DateTime.TryParse(testContext.Properties["monthIntervalInvalidEndDate"]?.ToString() ?? String.Empty, out monthIntervalInvalidEndDate);
            validMonthlyDebtEvolutionYear = Convert.ToInt32(testContext.Properties["validMonthlyEvolutionYear"]?.ToString() ?? String.Empty);
            invalidMonthlyDebtEvolutionYear = Convert.ToInt32(testContext.Properties["invalidMonthlyEvolutionYear"]?.ToString() ?? String.Empty);
            singleMonthDebtPercentageOwedToFred = Convert.ToDouble(testContext.Properties["singleMonthDebtPercentageOwedToFred"]?.ToString() ?? String.Empty);
            singleMonthDebtValueOwedToFred = Convert.ToInt32(testContext.Properties["singleMonthDebtValueOwedToFred"]?.ToString() ?? String.Empty);
            singleMonthDebtPercentageOwedToJonathan = Convert.ToDouble(testContext.Properties["singleMonthDebtPercentageOwedToJonathan"]?.ToString() ?? String.Empty);
            singleMonthDebtValueOwedToJonathan = Convert.ToInt32(testContext.Properties["singleMonthDebtValueOwedToJonathan"]?.ToString() ?? String.Empty);
            multipleMonthsDebtPercentageOwedToAndrew = Convert.ToDouble(testContext.Properties["multipleMonthsDebtPercentageOwedToAndrew"]?.ToString() ?? String.Empty);
            multipleMonthsDebtValueOwedToAndrew = Convert.ToInt32(testContext.Properties["multipleMonthsDebtValueOwedToAndrew"]?.ToString() ?? String.Empty);
            multipleMonthsDebtPercentageOwedToFred = Convert.ToDouble(testContext.Properties["multipleMonthsDebtPercentageOwedToFred"]?.ToString() ?? String.Empty);
            multipleMonthsDebtValueOwedToFred = Convert.ToInt32(testContext.Properties["multipleMonthsDebtValueOwedToFred"]?.ToString() ?? String.Empty);
            multipleMonthsDebtPercentageOwedToJonathan = Convert.ToDouble(testContext.Properties["multipleMonthsDebtPercentageOwedToJonathan"]?.ToString() ?? String.Empty);
            multipleMonthsDebtValueOwedToJonathan = Convert.ToInt32(testContext.Properties["multipleMonthsDebtValueOwedToJonathan"]?.ToString() ?? String.Empty);
            januaryMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["januaryMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            februaryMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["februaryMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            marchMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["marchMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            aprilMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["aprilMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            mayMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["mayMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            juneMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["juneMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            julyMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["julyMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            augustMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["augustMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            septemberMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["septemberMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            octoberMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["octoberMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            novemberMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["novemberMonthlyTotalDebts"]?.ToString() ?? String.Empty);
            decemberMonthlyTotalDebts = Convert.ToInt32(testContext.Properties["decemberMonthlyTotalDebts"]?.ToString() ?? String.Empty);
        }

        public void GetSingleMonthDebtList_WhenDataFound_DateMatchesInput() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<DebtDto> debtList = debtQueryService.GetDebtsByUserIdAndDateInterval(singleMonthValidStartDate, singleMonthValidEndDate);

            Assert.IsNotEmpty(debtList);

            bool hasCorrectDebtsDate = true;
            foreach (DebtDto debt in debtList) {
                int currentDay = debt.Date.Day;
                int currentYear = debt.Date.Year;

                int firstDayOfMonth = singleMonthValidStartDate.Day;
                int lastDayOfMonth = singleMonthValidEndDate.Day;
                int year = singleMonthValidStartDate.Year;

                if (currentDay < firstDayOfMonth || currentDay > lastDayOfMonth || currentYear != year) {
                    hasCorrectDebtsDate = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectDebtsDate);
        }

        [TestMethod]
        public void GetSingleMonthDebtList_WhenNoDataFound_DebtListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection = new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<DebtDto> debtList = debtQueryService.GetDebtsByUserIdAndDateInterval(singleMonthInvalidStartDate, singleMonthInvalidEndDate);

            Assert.IsEmpty(debtList);
        }

        [TestMethod]
        public void GetMonthIntervalDebtList_WhenDataFound_DateMatchesInput() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<DebtDto> debtList = debtQueryService.GetDebtsByUserIdAndDateInterval(monthIntervalValidStartDate, monthIntervalValidEndDate);

            Assert.IsNotEmpty(debtList);

            bool hasCorrectDebtsDate = true;
            foreach (DebtDto debt in debtList) {
                int currentMonth = debt.Date.Month;
                int currentYear = debt.Date.Year;

                int firstMonthOfInterval = monthIntervalValidStartDate.Month;
                int lastMonthOfInterval = monthIntervalValidEndDate.Month;
                int year = monthIntervalValidStartDate.Year;

                if (currentMonth < firstMonthOfInterval || currentMonth > lastMonthOfInterval || currentYear != year) {
                    hasCorrectDebtsDate = false;
                    break;
                }
            }

            Assert.IsTrue(hasCorrectDebtsDate);
        }

        [TestMethod]
        public void GetMonthIntervalDebtList_WhenNoDataFound_DebtListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            List<DebtDto> debtList = debtQueryService.GetDebtsByUserIdAndDateInterval(monthIntervalInvalidStartDate, monthIntervalInvalidEndDate);

            Assert.IsEmpty(debtList);
        }

        [TestMethod]
        public void GetSingleMonthCreditorStatistics_WhenDataFound_StatisticsDataMatches() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemCategoriesStatisticsDto debtCreditorStatistics = debtQueryService.GetAggregatedDebtsByCreditor(singleMonthInvalidStartDate, singleMonthInvalidEndDate);

            foreach (CategoryStatisticsDto debtCreditorStatisticDto in debtCreditorStatistics.CategoriesStatistics) {
                switch (debtCreditorStatisticDto.Name) {
                    case "Fred":
                        Assert.AreEqual(singleMonthDebtPercentageOwedToFred, debtCreditorStatisticDto.Percentage);
                        Assert.AreEqual(singleMonthDebtValueOwedToFred, debtCreditorStatisticDto.Value);
                        break;

                    case "Jonathan":
                        Assert.AreEqual(singleMonthDebtPercentageOwedToJonathan, debtCreditorStatisticDto.Percentage);
                        Assert.AreEqual(singleMonthDebtValueOwedToJonathan, debtCreditorStatisticDto.Value);
                        break;

                    default:
                        Assert.Fail($"Unknown creditor found:{debtCreditorStatisticDto.Name}");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetSingleMonthCreditorStatistics_WhenNoDataFound_CreditorStatisticsListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemCategoriesStatisticsDto debtCreditorStatistics = debtQueryService.GetAggregatedDebtsByCreditor(singleMonthInvalidStartDate, singleMonthInvalidEndDate);

            List<CategoryStatisticsDto> debtCreditorStatisticsList = debtCreditorStatistics.CategoriesStatistics;

            Assert.IsEmpty(debtCreditorStatisticsList);
        }

        [TestMethod]
        public void GetMonthIntervalCreditorStatistics_WhenDataFound_StatisticsDataMatches() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemCategoriesStatisticsDto debtCreditorStatistics = debtQueryService.GetAggregatedDebtsByCreditor(monthIntervalValidStartDate, monthIntervalValidEndDate);

            foreach (CategoryStatisticsDto debtCreditorStatisticDto in debtCreditorStatistics.CategoriesStatistics) {
                switch (debtCreditorStatisticDto.Name) {
                    case "Andrew":
                        Assert.AreEqual(multipleMonthsDebtPercentageOwedToAndrew, debtCreditorStatisticDto.Percentage);
                        Assert.AreEqual(multipleMonthsDebtValueOwedToAndrew, debtCreditorStatisticDto.Value);
                        break;

                    case "Fred":
                        Assert.AreEqual(multipleMonthsDebtPercentageOwedToFred, debtCreditorStatisticDto.Percentage);
                        Assert.AreEqual(multipleMonthsDebtValueOwedToFred, debtCreditorStatisticDto.Value);
                        break;

                    case "Jonathan":
                        Assert.AreEqual(multipleMonthsDebtPercentageOwedToJonathan, debtCreditorStatisticDto.Percentage);
                        Assert.AreEqual(multipleMonthsDebtValueOwedToJonathan, debtCreditorStatisticDto.Value);
                        break;

                    default:
                        Assert.Fail($"Unknown creditor found:{debtCreditorStatisticDto.Name}");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetMonthIntervalCreditorStatistics_WhenNoDataFound_CreditorStatisticsListIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemCategoriesStatisticsDto debtCreditorStatistics = debtQueryService.GetAggregatedDebtsByCreditor(monthIntervalInvalidStartDate, monthIntervalInvalidEndDate);

            List<CategoryStatisticsDto> debtCreditorStatisticsList = debtCreditorStatistics.CategoriesStatistics;

            Assert.IsEmpty(debtCreditorStatisticsList);
        }


        [TestMethod]
        public void GetMonthlyDebtEvolution_WhenDataFound_MonthlyDebtValuesMatch() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemMonthlyEvolutionDto monthlyDebtEvolutionDto = debtQueryService.GetMonthlyDebtsEvolution(validMonthlyDebtEvolutionYear);

            Dictionary<Month, int> monthlyDebtStatistics = monthlyDebtEvolutionDto.MonthlyStatistics;
            foreach (KeyValuePair<Month, int> monthlyDebt in monthlyDebtStatistics.ToList()) {
                switch (monthlyDebt.Key) {
                    case Month.January:
                        Assert.AreEqual(januaryMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.February:
                        Assert.AreEqual(februaryMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.March:
                        Assert.AreEqual(marchMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.April:
                        Assert.AreEqual(aprilMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.May:
                        Assert.AreEqual(mayMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.June:
                        Assert.AreEqual(juneMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.July:
                        Assert.AreEqual(julyMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.August:
                        Assert.AreEqual(augustMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.September:
                        Assert.AreEqual(septemberMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.October:
                        Assert.AreEqual(octoberMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.November:
                        Assert.AreEqual(novemberMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    case Month.December:
                        Assert.AreEqual(decemberMonthlyTotalDebts, monthlyDebt.Value);
                        break;

                    default:
                        Assert.Fail("Invalid month value retrieved from the response object.");
                        break;
                }
            }
        }

        [TestMethod]
        public void GetMonthlyDebtsEvolution_WhenNoDataFound_MonthlyDebtStatisticsIsEmpty() {
            IUserSessionService userSessionService = Substitute.For<IUserSessionService>();
            MySqlConnection mySqlTestConnection =
                    new MySqlConnection(mySqlDbContainer.GetConnectionString());
            MySqlTestDatabaseConnection mySqlTestDatabaseConnection = new MySqlTestDatabaseConnection(mySqlTestConnection);
            DebtQueryService debtQueryService = new DebtQueryService(mySqlTestDatabaseConnection, userSessionService);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser(validUserId, validEmailAddress);
            userSessionService.AuthenticatedUser.Returns(authenticatedUser);

            BudgetItemMonthlyEvolutionDto monthlyDebtEvolutionDto = debtQueryService.GetMonthlyDebtsEvolution(invalidMonthlyDebtEvolutionYear);

            Dictionary<Month, int> monthlyDebtStatistics = monthlyDebtEvolutionDto.MonthlyStatistics;

            Assert.IsEmpty(monthlyDebtStatistics);
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
