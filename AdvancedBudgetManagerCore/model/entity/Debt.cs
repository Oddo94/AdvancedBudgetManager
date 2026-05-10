using System;

namespace AdvancedBudgetManagerCore.model.entity {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the debt entity from the database.
    /// </summary>
    public class Debt {
        /// <summary>
        /// The debt id.
        /// </summary>
        private long debtId;

        /// <summary>
        /// The user id.
        /// </summary>
        private long userId;

        /// <summary>
        /// The debt name.
        /// </summary>
        private string name;

        /// <summary>
        /// The debt value.
        /// </summary>
        private int value;

        /// <summary>
        /// The creditor id.
        /// </summary>
        private long creditorId;

        /// <summary>
        /// The debt date.
        /// </summary>
        private DateTime date;


        /// <summary>
        /// Initializes a new instance of the <see cref="Debt"/> entity based on the provided parameters.
        /// </summary>
        /// <param name="debtId">The debt id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="name">The debt name.</param>
        /// <param name="value">The debt value.</param>
        /// <param name="creditorId">The creditor id.</param>
        /// <param name="date">The debt date.</param>
        public Debt(long debtId, long userId, string name, int value, long creditorId, DateTime date) {
            this.debtId = debtId;
            this.userId = userId;
            this.name = name;
            this.value = value;
            this.creditorId = creditorId;
            this.date = date;
        }

        public long DebtId {
            get { return this.debtId; }
            set { this.debtId = value; }
        }

        public long UserId {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public long CreditorId {
            get { return this.creditorId; }
            set { this.creditorId = value; }
        }

        public DateTime Date {
            get { return this.date; }
            set { this.date = value; }
        }
    }
}
