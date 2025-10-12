using AdvancedBudgetManagerCore.utils.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.request {
    /// <summary>
    /// Provides functionality for performing update requests to the database.
    /// </summary>
    public interface IDataUpdateRequest {
        /// <summary>
        /// Retrieves the request type.
        /// </summary>
        /// <returns>A <see cref="DataUpdateRequestType"/> enum containing the actual request type</returns>
        DataUpdateRequestType GetDataUpdateRequestType();

        /// <summary>
        /// Retrieves the parameter value used to updated the data.
        /// </summary>
        /// <returns>A <see cref="String"/> containing the parameter value</returns>
        string GetUpdateParameter();
    }
}
