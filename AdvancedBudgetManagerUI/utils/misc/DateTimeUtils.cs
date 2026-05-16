using System;

namespace AdvancedBudgetManager.utils.misc {
    public class DateTimeUtils {
        public DateTimeUtils() { }
        public DateRange? GetMonthRange(DateTimeOffset? inputStartDate, DateTimeOffset? inputEndDate, bool isMonthInterval) {
            if (inputStartDate == null) {
                return null;
            }

            DateTime selectedStartDate = inputStartDate.Value.DateTime;

            DateTime normalizedStartDate;
            DateTime normalizedEndDate;
            if (isMonthInterval && inputEndDate != null) {
                DateTime selectedEndDate = inputEndDate.Value.DateTime;

                normalizedStartDate = new DateTime(selectedStartDate.Year, selectedStartDate.Month, 1);
                normalizedEndDate = new DateTime(selectedEndDate.Year, selectedEndDate.Month, DateTime.DaysInMonth(selectedEndDate.Year, selectedEndDate.Month)
                ).AddDays(1).AddTicks(-1);
            } else {
                normalizedStartDate = new DateTime(selectedStartDate.Year, selectedStartDate.Month, 1);
                normalizedEndDate = normalizedStartDate.AddMonths(1).AddTicks(-1);
            }

            return new DateRange(normalizedStartDate, normalizedEndDate);
        }
    }
}
