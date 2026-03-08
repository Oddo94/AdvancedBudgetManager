using System;

namespace AdvancedBudgetManagerCore.model.entity {
    public class Income {
        private long incomeId;
        private long userId;
        private string name;
        private long incomeType;
        private int value;
        private DateTime date;

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
