using MySql.Data.MySqlClient;

namespace AdvancedBudgetManagerTest.integration.utils {
    public class DatabaseSeed {

        public static async Task PopulateDb(string scriptPath, MySqlConnection conn) {
            string scriptStatements = File.ReadAllTextAsync(scriptPath).Result;

            MySqlCommand scriptExecutionCommand = new MySqlCommand(scriptStatements, conn);

            await scriptExecutionCommand.ExecuteNonQueryAsync();
        }
    }
}
