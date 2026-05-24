using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.dto {
    public class MonthlyStatisticsDto {
        private Month month;
        private int value;

        public MonthlyStatisticsDto(Month month, int value) {
            this.month = month;
            this.value = value;
        }

        public Month Month {
            get { return this.month; }
            set { this.month = value; }
        }

        public int Value {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
