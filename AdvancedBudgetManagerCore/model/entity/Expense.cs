using System;

namespace AdvancedBudgetManagerCore.model.entity {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the expense entity from the database.
    /// </summary>
    public class Expense {
        /// <summary>
        /// The expense id.
        /// </summary>
        private long expenseId;

        /// <summary>
        /// The user ID.
        /// </summary>
        private long userId;

        /// <summary>
        /// The expense name.
        /// </summary>
        private string name;

        /// <summary>
        /// The expense type.
        /// </summary>
        private long type;

        /// <summary>
        /// The expense value.
        /// </summary>
        private int value;

        /// <summary>
        /// The expense date.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> entity based on the provided parameters.
        /// </summary>
        /// <param name="expenseId">The expense id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="name">The expense name.</param>
        /// <param name="type">The expense type.</param>
        /// <param name="value">The expense value.</param>
        /// <param name="date">The expense date.</param>
        public Expense(long expenseId, long userId, string name, long type, int value, DateTime date) {
            this.expenseId = expenseId;
            this.userId = userId;
            this.name = name;
            this.type = type;
            this.value = value;
            this.date = date;
        }

        public long ExpenseId {
            get { return this.expenseId; }
            set { this.expenseId = value; }
        }

        public long UserId {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public long Type {
            get { return this.type; }
            set { this.type = value; }
        }

        public int Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public DateTime Date {
            get { return this.date; }
            set { this.date = value; }
        }
    }
}
