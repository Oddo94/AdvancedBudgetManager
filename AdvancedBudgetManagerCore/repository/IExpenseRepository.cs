using AdvancedBudgetManagerCore.model.entity;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    public interface IExpenseRepository : ICrudRepository<Expense, long> {
        public List<Expense> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate);
        public List<Expense> GetAllLikeName(long userId, string name);
        public Expense GetByName(long userId, string name);
    }
}
