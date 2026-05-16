using System;

namespace AdvancedBudgetManagerCore.model.entity {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the income entity from the database.
    /// </summary>
    public class Income {
        /// <summary>
        /// The income id.
        /// </summary>
        private long incomeId;

        /// <summary>
        /// The user id.
        /// </summary>
        private long userId;

        /// <summary>
        /// The username.
        /// </summary>
        private string name;

        /// <summary>
        /// The income type.
        /// </summary>
        private long incomeType;

        /// <summary>
        /// The income value.
        /// </summary>
        private int value;

        /// <summary>
        /// The income date.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Initializes a new instance of the <see cref="Income"/> entity based on the provided parameters.
        /// </summary>
        /// <param name="incomeId">The income id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="name">The income name.</param>
        /// <param name="incomeType">The income type.</param>
        /// <param name="value">The income value.</param>
        /// <param name="date">The income date.</param>
        public Income(long incomeId, long userId, string name, long incomeType, int value, DateTime date) {
            this.incomeId = incomeId;
            this.userId = userId;
            this.name = name;
            this.incomeType = incomeType;
            this.value = value;
            this.date = date;
        }

        public long IncomeId {
            get { return this.incomeId; }
            set { this.incomeId = value; }
        }

        public long UserId {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public long IncomeType {
            get { return this.incomeType; }
            set { this.incomeType = value; }
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
