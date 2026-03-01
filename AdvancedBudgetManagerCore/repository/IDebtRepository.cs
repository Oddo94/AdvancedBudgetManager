using AdvancedBudgetManagerCore.model.entity;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    public interface IDebtRepository : ICrudRepository<Debt, long> {
        public List<Debt> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate);
        public List<Debt> GetAllLikeName(long userId, string name);
        public Debt GetByName(long userId, string name);
    }
}
