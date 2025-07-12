using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.request {
    /// <summary>
    /// Represents a request that encapsulates the parameters used for data retrieval
    /// </summary>
    public interface IDataRequest {  
        /// <summary>
        /// Provides the actual type of the data request
        /// </summary>
        /// <returns>A <see cref="DataRequestType"/> containing the request type</returns>
        DataRequestType GetDataRequestType();

        /// <summary>
        /// Retrieves the search parameter
        /// </summary>
        /// <returns>A <see cref="string"/> containing the value of the search parameter</returns>
        string GetSearchParameter();
    }
}
