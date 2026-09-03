using System;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for storing the information related to the user's expenses.
    /// </summary>
    public class ExpenseDto {
        /// <summary>
        /// The expense name.
        /// </summary>
        private string name;

        /// <summary>
        /// The expense type.
        /// </summary>
        private string type;

        /// <summary>
        /// The expense value.
        /// </summary>
        private long value;

        /// <summary>
        /// The expense date.
        /// </summary>
        private DateTime date;


        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseDto"/> based on the provided arguments.
        /// </summary>
        /// <param name="name">The expense name.</param>
        /// <param name="type">The expense type.</param>
        /// <param name="value">The expense value.</param>
        /// <param name="date">The expense date.</param>
        public ExpenseDto(string name, string type, long value, DateTime date) {
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
