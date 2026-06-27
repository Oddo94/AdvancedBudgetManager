using AdvancedBudgetManagerTest.integration.utils;
using MySql.Data.MySqlClient;
using System.Data;

namespace AdvancedBudgetManagerTest.integration.service {
    [TestClass]
    public class IncomeQueryServiceIntegrationTests : IntegrationTestBase {
        private static MySqlConnection mySqlTestConnection;

        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            try {
                IntegrationTestBase testBase = new IntegrationTestBase();

                testBase.Initialize().GetAwaiter().GetResult();


                mySqlTestConnection =
                    new MySqlConnection(testBase.MySqlDbContainer.GetConnectionString());
                mySqlTestConnection.Open();

                DatabaseSchema.CreateAsync(mySqlTestConnection)
                    .GetAwaiter()
                    .GetResult();
            } catch (Exception ex) {
                throw new Exception("Class initialization failed", ex);
            }
        }

        [TestMethod]
        public void TestDatabaseCreation() {
            string getAllIncomesQuery = "SELECT * FROM incomes";

            MySqlCommand getAllIncomesCommand = new MySqlCommand(getAllIncomesQuery, mySqlTestConnection);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getAllIncomesCommand);
            DataTable allIncomes = new DataTable();

            dataAdapter.Fill(allIncomes);

            int count = allIncomes.Rows.Count;

            Assert.AreEqual(0, count);
        }

        //[TestMethod]
        //public void CanStartDbContainer() {
        //    Assert.AreEqual(TestcontainersStates.Running, mySqlDbContainer.State);
        //}

        [ClassCleanup]
        public static void Cleanup() {
            mySqlTestConnection?.Dispose();
        }
    }
}
