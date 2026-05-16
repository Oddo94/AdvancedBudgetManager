using System;

namespace AdvancedBudgetManagerCore.model.entity {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the saving entity from the database.
    /// </summary>
    public class Saving {
        /// <summary>
        /// The saving id.
        /// </summary>
        private long savingId;

        /// <summary>
        /// The user id.
        /// </summary>
        private long userId;

        /// <summary>
        /// The saving name.
        /// </summary>
        private string name;

        /// <summary>
        /// The saving value. 
        /// </summary>
        private int value;

        /// <summary>
        /// The saving date.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saving"/> entity based on the provided parameters.
        /// </summary>
        /// <param name="savingId">The saving id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="name">The saving name.</param>
        /// <param name="value">The saving value.</param>
        /// <param name="date">The saving date.</param>
        public Saving(long savingId, long userId, string name, int value, DateTime date) {
            this.savingId = savingId;
            this.userId = userId;
            this.name = name;
            this.value = value;
            this.date = date;
        }

        public long SavingId {
            get { return this.savingId; }
            set { this.savingId = value; }
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

        public DateTime Date {
            get { return this.date; }
            set { this.date = value; }
        }
    }
}
