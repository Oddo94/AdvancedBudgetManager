using System;

namespace AdvancedBudgetManagerCore.model.dto {
    public class DebtDto {
        private string name;
        private string creditorName;
        private long value;
        private DateTime date;

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
