using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.request {
    public interface IDataRequest {     
        DataRequestType GetDataRequestType();
        string GetSearchParameter();
    }
}
