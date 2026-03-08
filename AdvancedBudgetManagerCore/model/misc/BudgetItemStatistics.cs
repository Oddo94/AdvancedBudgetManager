namespace AdvancedBudgetManagerCore.model.misc {
    public class BudgetItemStatistics {
        private int totalValue;
        private double totalPercentage;

        public BudgetItemStatistics(int totalValue, double totalPercentage) {
            this.totalValue = totalValue;
            this.totalPercentage = totalPercentage;
        }

        public int TotalValue {
            get { return this.totalValue; }
            set { this.totalValue = value; }
        }

        public double TotalPercentage {
            get { return this.totalPercentage; }
            set { this.totalPercentage = value; }
        }
    }
}
