namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used to retieve the information related to the budget summary.
    /// </summary>
    public class BudgetSummaryDto {
        /// <summary>
        /// The total incomes.
        /// </summary>
        private int totalIncomes;

        /// <summary>
        /// The total incomes percentage.
        /// </summary>
        private double totalIncomesPercentage;

        /// <summary>
        /// The total expenses.
        /// </summary>
        private int totalExpenses;

        /// <summary>
        /// The total expenses percentage.
        /// </summary>
        private double totalExpensesPercentage;

        /// <summary>
        /// The total debts.
        /// </summary>
        private int totalDebts;

        /// <summary>
        /// The total debts percentage.
        /// </summary>
        private double totalDebtsPercentage;

        /// <summary>
        /// The total savings.
        /// </summary>
        private int totalSavings;

        /// <summary>
        /// The total savings percentage.
        /// </summary>
        private double totalSavingsPercentage;

        /// <summary>
        /// The total left to spend.
        /// </summary>
        private int totalLeftToSpend;

        /// <summary>
        /// The total left to spend percentage.
        /// </summary>
        private double totalLeftToSpendPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetSummaryDto"/> based on the provided parameters.
        /// </summary>
        /// <param name="totalIncomes">The total incomes.</param>
        /// <param name="totalIncomesPercentage">The total incomes percentage</param>
        /// <param name="totalExpenses">The total expenses.</param>
        /// <param name="totalExpensesPercentage">The total expenses percentage.</param>
        /// <param name="totalDebts">The total debts.</param>
        /// <param name="totalDebtsPercentage">The total debts percentage.</param>
        /// <param name="totalSavings">The total savings.</param>
        /// <param name="totalSavingsPercentage">The total savings percentage.</param>
        /// <param name="totalLeftToSpend">The total left to spend.</param>
        /// <param name="totalLeftToSpendPercentage">The total left to spend percentage.</param>
        /// <remarks>The total percentage value for each item (income, expense, debt, saving) is calculated based on the total incomes.</remarks>
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