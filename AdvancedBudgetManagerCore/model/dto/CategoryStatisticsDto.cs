namespace AdvancedBudgetManagerCore.model.dto {
    public class CategoryStatisticsDto {
        private string name;
        private int value;
        private double percentage;


        public CategoryStatisticsDto(string name, int value, double percentage) {
            this.name = name;
            this.value = value;
            this.percentage = percentage;
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public double Percentage {
            get { return this.percentage; }
            set { this.percentage = value; }
        }
    }
}
