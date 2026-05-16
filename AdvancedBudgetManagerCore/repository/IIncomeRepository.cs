using AdvancedBudgetManagerCore.model.entity;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    /// <summary>
    /// Interface that specifies the additional operations which should be implemented by an income repository.
    /// </summary>
    public interface IIncomeRepository : ICrudRepository<Income, long> {
        /// <summary>
        /// Retrieves a list of incomes based on a user ID and a date interval.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="startDate">The start date of the date interval.</param>
        /// <param name="endDate">The end date of the date interval.</param>
        /// <returns>A list of <see cref="Income"/> objects.</returns>
        public List<Income> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retrieves a list of incomes whose name is similar to the provided one, based on a user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="name">The approximate name of the income.</param>
        /// <returns>A list of <see cref="Income"/> objects.</returns>
        public List<Income> GetAllLikeName(long userId, string name);

        /// <summary>
        /// Retrieves an income based on the user ID and provided name.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="name">The exact name of the income.</param>
        /// <returns>The <see cref="Income"/> object that was retrieved.</returns>
        public Income GetByName(long userId, string name);
    }
}
