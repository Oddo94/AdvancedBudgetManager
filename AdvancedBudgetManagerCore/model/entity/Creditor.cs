namespace AdvancedBudgetManagerCore.model.entity {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the creditor entity from the database.
    /// </summary>
    public class Creditor {
        /// <summary>
        /// The creditor id.
        /// </summary>
        private long creditorId;

        /// <summary>
        /// The creditor name.
        /// </summary>
        private string creditorName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Creditor"/> entity based on the provided parameters.
        /// </summary>
        /// <param name="creditorId">The creditor id.</param>
        /// <param name="creditorName">The creditor name.</param>
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
