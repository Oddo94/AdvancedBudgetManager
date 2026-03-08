using AdvancedBudgetManagerCore.model.entity;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    public interface IIncomeRepository : ICrudRepository<Income, long> {
        public List<Income> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate);
        public List<Income> GetAllLikeName(long userId, string name);
        public Income GetByName(long userId, string name);
    }
}
