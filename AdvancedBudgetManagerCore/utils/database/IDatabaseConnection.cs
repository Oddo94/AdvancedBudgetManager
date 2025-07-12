using System.Data;

namespace AdvancedBudgetManagerCore.utils.database {
    /// <summary>
    /// Represents the abstract database connection used by all classes that interact with the database.
    /// </summary>
    public interface IDatabaseConnection {
        /// <summary>
        /// Provides the connection to the database.
        /// </summary>
        /// <returns>A <see cref="IDbConnection"/> which contains the database connection.</returns>
        IDbConnection GetConnection();
    }
}
