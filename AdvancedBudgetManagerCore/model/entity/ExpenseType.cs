namespace AdvancedBudgetManagerCore.model.entity {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the expense type entity from the database.
    /// </summary>
    public class ExpenseType {
        /// <summary>
        /// The category id.
        /// </summary>
        private long categoryId;

        /// <summary>
        /// The category name.
        /// </summary>
        private string categoryName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseType"/> entity based on the provided parameters.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="categoryName">The category name.</param>
        public ExpenseType(long categoryId, string categoryName) {
            this.categoryId = categoryId;
            this.categoryName = categoryName;
        }

        public long CategoryId {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }

        public string CategoryName {
            get { return this.categoryName; }
            set { this.categoryName = value; }
        }
    }
}
