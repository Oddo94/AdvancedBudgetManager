using AdvancedBudgetManagerCore.model.entity;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    public interface ISavingRepository : ICrudRepository<Saving, long> {
        public List<Saving> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate);
        public List<Saving> GetAllLikeName(long userId, string name);
        public Saving GetByName(long userId, string name);
    }
}
