using MySql.Data.MySqlClient;

namespace AdvancedBudgetManagerTest.integration.utils {
    public static class DatabaseSchema {

        public static async Task CreateAsync(MySqlConnection conn) {
            string sqlScript = File.ReadAllTextAsync(".\\resources\\scripts\\mysql_schema.sql").Result;

            MySqlCommand scriptExecutionCommand = new MySqlCommand(sqlScript, conn);

            await scriptExecutionCommand.ExecuteNonQueryAsync();
        }
    }
}
