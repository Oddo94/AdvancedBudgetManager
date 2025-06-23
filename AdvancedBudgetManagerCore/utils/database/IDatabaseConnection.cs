using System.Data;

namespace AdvancedBudgetManagerCore.utils.database {
    public interface IDatabaseConnection {
        IDbConnection GetConnection();
    }
}
