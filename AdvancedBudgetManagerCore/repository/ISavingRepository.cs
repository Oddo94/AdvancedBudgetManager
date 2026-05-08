using AdvancedBudgetManagerCore.model.entity;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    /// <summary>
    /// Interface that specifies the additional operations that should be implemented by a saving repository.
    /// </summary>
    public interface ISavingRepository : ICrudRepository<Saving, long> {
        /// <summary>
        /// Retrieves a list of savings based on a user ID and a date interval.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A <see cref="List{Saving}"/> that contains the retrieved savings.</returns>
        public List<Saving> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retrieves a list of savings whose name is similar to the provided one, based on a user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="name">The approximate name of the saving.</param>
        /// <returns>A <see cref="List{Saving}"/> that contains the retrieved savings.</returns>
        public List<Saving> GetAllLikeName(long userId, string name);

        /// <summary>
        /// Retrieves a saving based on the provided name and user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="name">The exact name of the saving.</param>
        /// <returns>The <see cref="Saving"/> entity that was retrieved.</returns>
        public Saving GetByName(long userId, string name);
    }
}
