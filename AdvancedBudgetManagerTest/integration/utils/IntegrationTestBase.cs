using Testcontainers.MySql;

namespace AdvancedBudgetManagerTest.integration.utils {
    public class IntegrationTestBase {
        private static MySqlContainer mySqlDbContainer;

        public IntegrationTestBase() { }
        public async Task Initialize() {
            mySqlDbContainer = new MySqlBuilder("mysql:8.4")
                .WithDatabase("test_budget_manager")
                .WithCleanUp(true)
                .Build();

            await mySqlDbContainer.StartAsync();
        }

        public async Task CleanupDbContainer() {
            await mySqlDbContainer.DisposeAsync();
        }

        public MySqlContainer MySqlDbContainer {
            get { return mySqlDbContainer; }
            set { mySqlDbContainer = value; }
        }
    }
}
