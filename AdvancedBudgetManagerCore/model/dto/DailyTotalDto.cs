namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the parent data transfer object used to retrieve the aggregated data related to daily totals for a specific entity (income, expense, debt, saving).
    /// </summary>
    public class DailyTotalDto {
        /// <summary>
        /// The day of month.
        /// </summary>
        private int day;

        /// <summary>
        /// The total aggregated value.
        /// </summary>
        private double totalValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyTotalDto"/> object with the default values.
        /// </summary>
        public DailyTotalDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyTotalDto"/> object based on the provided parameters.
        /// </summary>
        /// <param name="day">The day of month.</param>
        /// <param name="totalValue">The total aggregated value.</param>
        public DailyTotalDto(int day, double totalValue) {
            this.day = day;
            this.totalValue = totalValue;
        }

        public int Day {
            get { return this.day; }
            set { this.day = value; }
        }

        public double TotalValue {
            get { return this.totalValue; }
            set { this.totalValue = value; }
        }
    }
}
