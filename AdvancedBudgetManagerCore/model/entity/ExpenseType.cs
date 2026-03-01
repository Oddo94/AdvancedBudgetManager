namespace AdvancedBudgetManagerCore.model.entity {
    public class ExpenseType {
        private long categoryId;
        private string categoryName;

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
