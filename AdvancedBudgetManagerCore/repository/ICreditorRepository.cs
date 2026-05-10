using AdvancedBudgetManagerCore.model.entity;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    /// <summary>
    /// Interface that specifies the additional operations which should be implemented by a creditor repository.
    /// </summary>
    public interface ICreditorRepository : ICrudRepository<Creditor, long> {
        /// <summary>
        /// Retrieves a list of creditors associated to a user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of <see cref="Creditor"/> objects.</returns>
        public List<Creditor> getAllCreditorsByUserId(long userId);
    }
}
