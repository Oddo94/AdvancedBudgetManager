namespace AdvancedBudgetManagerCore.model.dto {
    public class BudgetSummaryDto {
        private int totalIncomes;
        private double totalIncomesPercentage;
        private int totalExpenses;
        private double totalExpensesPercentage;
        private int totalDebts;
        private double totalDebtsPercentage;
        private int totalSavings;
        private double totalSavingsPercentage;
        private int totalLeftToSpend;
        private double totalLeftToSpendPercentage;

        public BudgetSummaryDto(int totalIncomes, double totalIncomesPercentage, int totalExpenses, double totalExpensesPercentage,
            int totalDebts, double totalDebtsPercentage, int totalSavings, double totalSavingsPercentage, int totalLeftToSpend,
            double totalLeftToSpendPercentage) {
            this.totalIncomes = totalIncomes;
            this.totalIncomesPercentage = totalIncomesPercentage;
            this.totalExpenses = totalExpenses;
            this.totalExpensesPercentage = totalExpensesPercentage;
            this.totalDebts = totalDebts;
            this.totalDebtsPercentage = totalDebtsPercentage;
            this.totalSavings = totalSavings;
            this.totalSavingsPercentage = totalSavingsPercentage;
            this.totalLeftToSpend = totalLeftToSpend;
            this.totalLeftToSpendPercentage = totalLeftToSpendPercentage;
        }

        public int TotalIncomes {
            get { return this.totalIncomes; }
            set { this.totalIncomes = value; }
        }

        public double TotalIncomesPercentage {
            get { return this.totalIncomesPercentage; }
            set { this.totalIncomesPercentage = value; }
        }
        public int TotalExpenses {
            get { return this.totalExpenses; }
            set { this.totalExpenses = value; }
        }

        public double TotalExpensesPercentage {
            get { return this.totalExpensesPercentage; }
            set { this.totalExpensesPercentage = value; }
        }

        public int TotalDebts {
            get { return this.totalDebts; }
            set { this.totalDebts = value; }
        }

        public double TotalDebtsPercentage {
            get { return this.totalDebtsPercentage; }
            set { this.totalDebtsPercentage = value; }
        }

        public int TotalSavings {
            get { return this.totalSavings; }
            set { this.totalSavings = value; }
        }

        public double TotalSavingsPercentage {
            get { return this.totalSavingsPercentage; }
            set { this.totalSavingsPercentage = value; }
        }

        public int TotalLeftToSpend {
            get { return this.totalLeftToSpend; }
            set { this.totalLeftToSpend = value; }
        }

        public double TotalLeftToSpendPercentage {
            get { return this.totalLeftToSpendPercentage; }
            set { this.totalLeftToSpendPercentage = value; }
        }
    }
}