using AdvancedBudgetManagerCore.model.entity;

namespace AdvancedBudgetManagerCore.repository {
    public interface IUserRepository : ICrudRepository<User, long> {
        public User GetByEmail(string email);

        public User GetByUserName(string userName);
    }
}
