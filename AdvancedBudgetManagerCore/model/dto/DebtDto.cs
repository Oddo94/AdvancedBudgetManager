using System;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for storing the information related to the user's debts.
    /// </summary>
    public class DebtDto {
        /// <summary>
        /// The debt name.
        /// </summary>
        private string name;

        /// <summary>
        /// The creditor's name.
        /// </summary>
        private string creditorName;

        /// <summary>
        /// The debt value.
        /// </summary>
        private long value;

        /// <summary>
        /// The debt date.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebtDto"/> based on the provided arguments.
        /// </summary>
        /// <param name="name">The debt name.</param>
        /// <param name="creditorName">The creditor's name.</param>
        /// <param name="value">The debt value.</param>
        /// <param name="date">The debt date.</param>
        public DebtDto(string name, string creditorName, long value, DateTime date) {
            this.name = name;
            this.creditorName = creditorName;
            this.value = value;
            this.date = date;
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public string CreditorName {
            get { return this.creditorName; }
            set { this.creditorName = value; }
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
