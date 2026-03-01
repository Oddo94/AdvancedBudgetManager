using System;

namespace AdvancedBudgetManagerCore.model.entity {
    public class Expense {
        private long expenseId;
        private long userId;
        private string name;
        private long type;
        private long value;
        private DateTime date;

        public Expense(long expenseId, long userId, string name, long type, long value, DateTime date) {
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
