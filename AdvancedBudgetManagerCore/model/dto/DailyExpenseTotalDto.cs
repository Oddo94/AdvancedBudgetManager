namespace AdvancedBudgetManagerCore.model.dto {
    public class DailyExpenseTotalDto : DailyTotalDto {

        public DailyExpenseTotalDto() : base() { }

        public DailyExpenseTotalDto(int day, double totalValue) : base(day, totalValue) { }
    }
}
