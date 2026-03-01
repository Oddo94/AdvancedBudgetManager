using System;

namespace AdvancedBudgetManagerCore.model.entity {
    public class Debt {
        private long debtId;
        private long userId;
        private string name;
        private int value;
        private long creditorId;
        private DateTime date;

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
