using AdvancedBudgetManagerCore.model.request;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public interface ICrudRepository {
        DataTable GetData(IDataRequest dataRequest);
    }
}
