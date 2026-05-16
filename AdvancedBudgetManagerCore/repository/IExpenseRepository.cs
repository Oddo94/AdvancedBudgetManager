using AdvancedBudgetManagerCore.model.dto;
using AdvancedBudgetManagerCore.model.entity;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    /// <summary>
    /// Interface that specifies the additional operations which should be implemented by an expense repository. 
    /// </summary>
    public interface IExpenseRepository : ICrudRepository<Expense, long> {
        /// <summary>
        /// Retrieves a list of expenses based on a user ID and a date interval.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="startDate">The start date of the time interval.</param>
        /// <param name="endDate">The end date of the time interval.</param>
        /// <returns>A list of <see cref="Expense"/> objects.</returns>
        public List<Expense> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retrieves a list of expenses whose name is similar to the provided one, based on a user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="name">The approximate name of the expense.</param>
        /// <returns>A list of <see cref="Expense"/> objects.</returns>
        public List<Expense> GetAllLikeName(long userId, string name);

        /// <summary>
        /// Retrieves an expense based on the provided name and user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="name">The exact name of the expense.</param>
        /// <returns>The <see cref="Expense"/> object that was retrieved.</returns>
        public Expense GetByName(long userId, string name);

        /// <summary>
        /// Retrieves a list of daily expenses based on a user ID and a date interval.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="startDate">The start date of the date interval.</param>
        /// <param name="endDate">The end date of the date interval.</param>
        /// <returns>A list of <see cref="DailyExpenseTotalDto"/> objects.</returns>
        public List<DailyExpenseTotalDto> GetDailyExpenseTotalsForDateInterval(long userId, DateTime startDate, DateTime endDate);
    }
}
