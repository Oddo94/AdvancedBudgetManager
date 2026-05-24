using System;

namespace AdvancedBudgetManagerCore.model.dto {
    public class IncomeDto {
        private string name;
        private string type;
        private long value;
        private DateTime date;

        public IncomeDto(string name, string type, long value, DateTime date) {
            this.name = name;
            this.type = type;
            this.value = value;
            this.date = date;
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Type {
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
