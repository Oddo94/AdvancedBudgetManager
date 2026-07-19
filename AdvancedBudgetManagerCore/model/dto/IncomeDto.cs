using System;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for storing the information related to the user's incomes.
    /// </summary>
    public class IncomeDto {
        /// <summary>
        /// The income name.
        /// </summary>
        private string name;

        /// <summary>
        /// The income type.
        /// </summary>
        private string type;

        /// <summary>
        /// The income value.
        /// </summary>
        private long value;

        /// <summary>
        /// The income date.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeDto"/> based on the provided arguments.
        /// </summary>
        /// <param name="name">The income name.</param>
        /// <param name="type">The income type.</param>
        /// <param name="value">The income value.</param>
        /// <param name="date">The income date.</param>
        public IncomeDto(string name, string type, long value, DateTime date) {
            this.name = name;
            this.type = type;
            this.value = value;
            this.date = date;
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Type {
            get { return this.type; }
            set { this.type = value; }
        }

        public long Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public DateTime Date {
            get { return this.date; }
            set { this.date = value; }
        }
    }
}
