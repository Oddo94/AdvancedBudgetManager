using System;

namespace AdvancedBudgetManagerCore.model.entity {
    public class Saving {
        private long savingId;
        private long userId;
        private string name;
        private int value;
        private DateTime date;

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
