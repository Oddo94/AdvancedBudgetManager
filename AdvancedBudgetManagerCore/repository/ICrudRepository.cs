using AdvancedBudgetManagerCore.model.request;
using System;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
   /// <summary>
   /// Represents a repository for performing CRUD operations on the database
   /// </summary>
    public interface ICrudRepository {
        /// <summary>
        /// Retrieves the specified information from the database
        /// </summary>
        /// <param name="dataRequest">The data request object containing the search parameter</param>
        /// <returns>A <see cref="DataTable"/> containing the retrieved data</returns>
        /// <exception cref="SystemException"></exception>
        public DataTable GetData(IDataRequest dataRequest);
    }
}
