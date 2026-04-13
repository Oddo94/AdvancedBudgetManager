namespace AdvancedBudgetManagerCore.model.dto {
    public class DailyTotalDto {
        private int day;
        private double totalValue;

        public DailyTotalDto() { }

        public DailyTotalDto(int day, double totalValue) {
            this.day = day;
            this.totalValue = totalValue;
        }

        public int Day {
            get { return this.day; }
            set { this.day = value; }
        }

        public double TotalValue {
            get { return this.totalValue; }
            set { this.totalValue = value; }
        }
    }
}
