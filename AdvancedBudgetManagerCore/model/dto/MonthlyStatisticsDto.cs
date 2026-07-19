using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for storing the information related to monthly statistics.
    /// </summary>
    public class MonthlyStatisticsDto {
        /// <summary>
        /// The month to which the statistics belong.
        /// </summary>
        private Month month;

        /// <summary>
        /// The actual item value.
        /// </summary>
        private int value;


        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyStatisticsDto"/> based on the provided <see cref="Month"/> and value.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="value">The value</param>
        public MonthlyStatisticsDto(Month month, int value) {
            this.month = month;
            this.value = value;
        }

        public Month Month {
            get { return this.month; }
            set { this.month = value; }
        }

        public int Value {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
