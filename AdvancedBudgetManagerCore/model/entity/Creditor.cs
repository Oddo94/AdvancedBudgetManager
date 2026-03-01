namespace AdvancedBudgetManagerCore.model.entity {
    public class Creditor {
        private long creditorId;
        private string creditorName;

        public Creditor(long creditorId, string creditorName) {
            this.creditorId = creditorId;
            this.creditorName = creditorName;
        }

        public long CreditorId {
            get { return this.creditorId; }
            set { this.creditorId = value; }
        }

        public string CreditorName {
            get { return this.creditorName; }
            set { this.creditorName = value; }
        }
    }
}
