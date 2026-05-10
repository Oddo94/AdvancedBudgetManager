namespace AdvancedBudgetManagerCore.model.dto {
    /// <summary>
    /// Represents the data transfer object used to retrieve the aggregated data for expense daily totals.
    /// </summary>
    public class DailyExpenseTotalDto : DailyTotalDto {

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyExpenseTotalDto"/> object with the default values.
        /// </summary>
        public DailyExpenseTotalDto() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyExpenseTotalDto"/> object based on the provided parameters.
        /// </summary>
        /// <param name="day">The day of month.</param>
        /// <param name="totalValue">The total aggregated value.</param>
        public DailyExpenseTotalDto(int day, double totalValue) : base(day, totalValue) { }
    }
}
