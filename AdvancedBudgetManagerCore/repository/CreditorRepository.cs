using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.repository {
    public class CreditorRepository : ICrudRepository<Creditor, long> {
        private IDatabaseConnection dbConnection;
        public IEnumerable<Creditor> GetAll() {
            throw new NotImplementedException();
        }

        public Creditor GetById(long id) {
            throw new NotImplementedException();
        }

        public Creditor Insert(Creditor entity) {
            throw new NotImplementedException();
        }

        public Creditor Update(Creditor entity) {
            throw new NotImplementedException();
        }

        public bool Delete(long id) {
            throw new NotImplementedException();
        }
    }
}
