using System;

namespace AdvancedBudgetManager.utils.misc {
    public class DateRange {
        private DateTime startDate;
        private DateTime endDate;

        public DateRange(DateTime startDate, DateTime endDate) {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public DateTime StartDate {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        public DateTime EndDate {
            get { return this.endDate; }
            set { this.endDate = value; }
        }
    }
}
