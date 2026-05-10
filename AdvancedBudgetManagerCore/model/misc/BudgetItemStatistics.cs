namespace AdvancedBudgetManagerCore.model.misc {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the object used to store the statistics for a specific budget item (income, expense, debt, saving).
    /// </summary>
    public class BudgetItemStatistics {
        /// <summary>
        /// The total value.
        /// </summary>
        private int totalValue;

        /// <summary>
        /// The total percentage.
        /// </summary>
        private double totalPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetItemStatistics"/> object based on the provided parameters.
        /// </summary>
        /// <param name="totalValue">The total value.</param>
        /// <param name="totalPercentage">The total percentage.</param>
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
