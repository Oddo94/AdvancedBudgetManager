namespace AdvancedBudgetManager.utils.misc {
    public class BudgetSummaryItem {
        private string itemName;
        private int totalValue;
        private double totalPercentage;

        public BudgetSummaryItem(string itemName, int totalValue, double totalPercentage) {
            this.itemName = itemName;
            this.totalValue = totalValue;
            this.totalPercentage = totalPercentage;
        }

        public string ItemName {
            get { return this.itemName; }
            set { this.itemName = value; }
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
