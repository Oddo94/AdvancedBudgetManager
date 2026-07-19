namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for storing the information related to category statistics for a specific budget item.  
    /// </summary>
    public class CategoryStatisticsDto {
        /// <summary>
        /// The category name.
        /// </summary>
        private string name;

        /// <summary>
        /// The category value.
        /// </summary>
        private int value;

        /// <summary>
        /// The category percentage.
        /// </summary>
        private double percentage;


        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryStatisticsDto"/> based on the provided arguments.
        /// </summary>
        /// <param name="name">The category name.</param>
        /// <param name="value">The category value.</param>
        /// <param name="percentage">The category percentage.</param>
        public CategoryStatisticsDto(string name, int value, double percentage) {
            this.name = name;
            this.value = value;
            this.percentage = percentage;
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public double Percentage {
            get { return this.percentage; }
            set { this.percentage = value; }
        }
    }
}
