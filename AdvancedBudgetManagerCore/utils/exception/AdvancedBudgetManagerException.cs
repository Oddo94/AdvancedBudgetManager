using System;

namespace AdvancedBudgetManagerCore.utils.exception {
    public class AdvancedBudgetManagerException : Exception {

        public AdvancedBudgetManagerException() { }

        public AdvancedBudgetManagerException(string message) : base(message) { }

        public AdvancedBudgetManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
