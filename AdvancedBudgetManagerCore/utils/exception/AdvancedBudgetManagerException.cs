using AdvancedBudgetManagerCore.utils.enums;
using System;

namespace AdvancedBudgetManagerCore.utils.exception {
    /// <summary>
    /// Represents a custom exception for the AdvancedBudgetManager application.
    /// </summary>
    public class AdvancedBudgetManagerException : Exception {
        /// <summary>
        /// The category type of the exception
        /// </summary>
        private ExceptionCategory Category { get; }

        /// <summary>
        /// Default no-args constructor.
        /// </summary>
        public AdvancedBudgetManagerException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedBudgetManagerException"/> based on the provided message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public AdvancedBudgetManagerException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedBudgetManagerException"/> based on the provided message and inner exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public AdvancedBudgetManagerException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedBudgetManagerException"/> based on the provided category, message and inner exception.
        /// </summary>
        /// <param name="category">The category type of the exception</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public AdvancedBudgetManagerException(ExceptionCategory category, string message, Exception innerException) : base(message, innerException) {
            Category = category;
        }
    }
}
