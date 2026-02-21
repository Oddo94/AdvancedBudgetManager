using AdvancedBudgetManagerCore.model.entity;

namespace AdvancedBudgetManagerCore.repository {
    /// <summary>
    /// Interface that specifies the additional operations which should be implemented by a user repository.
    /// </summary>
    public interface IUserRepository : ICrudRepository<User, long> {
        /// <summary>
        /// Retrieves a user by its email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the user.</param>
        /// <returns>A <see cref="User"/> entity.</returns>
        public User GetByEmail(string emailAddress);

        /// <summary>
        /// Retrieves a user by its user name.
        /// </summary>
        /// <param name="userName">The user name of the user.</param>
        /// <returns>A <see cref="User"/> entity.</returns>
        public User GetByUserName(string userName);
    }
}
