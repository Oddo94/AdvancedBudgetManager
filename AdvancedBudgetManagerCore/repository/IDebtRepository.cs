using AdvancedBudgetManagerCore.model.entity;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    /// <summary>
    /// Interface that specifies the additional operations which should be implemented by a debt repository.
    /// </summary>
    public interface IDebtRepository : ICrudRepository<Debt, long> {
        /// <summary>
        /// Retrieves a list of debts based on a user ID and a date interval.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="startDate">The start date of the date interval.</param>
        /// <param name="endDate">The end date of the date interval.</param>
        /// <returns>A list of <see cref="Debt"/> objects.</returns>
        public List<Debt> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retrieves a list of debts whose name is similar to the provided one, based on a user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="name">The approximate name of the debt.</param>
        /// <returns>A list of <see cref="Debt"/> objects.</returns>
        public List<Debt> GetAllLikeName(long userId, string name);

        /// <summary>
        /// Retrieves a debt based on the user ID and provided name.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="name">The exact name of the income.</param>
        /// <returns>The <see cref="Income"/> object that was retrieved.</returns>
        public Debt GetByName(long userId, string name);
    }
}
