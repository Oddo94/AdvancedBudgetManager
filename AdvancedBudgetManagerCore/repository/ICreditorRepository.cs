using AdvancedBudgetManagerCore.model.entity;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    public interface ICreditorRepository : ICrudRepository<Creditor, long> {
        public IEnumerable<Creditor> getAllCreditorsByUserId(long userId);
    }
}
