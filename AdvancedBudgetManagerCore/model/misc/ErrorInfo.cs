using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.misc {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the object used to store the information related to a specific error.
    /// </summary>
    public class ErrorInfo {
        /// <summary>
        /// The error title.
        /// </summary>
        private string title;

        /// <summary>
        /// The error message.
        /// </summary>
        private string message;

        /// <summary>
        /// The error severity level.
        /// </summary>
        private ErrorSeverity severity;

        /// <summary>
        /// Default no-args constructor.
        /// </summary>
        public ErrorInfo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInfo"/> class based on the provided parameters.
        /// </summary>
        /// <param name="title">The error title.</param>
        /// <param name="message">The error message.</param>
        /// <param name="severity">The error severity level.</param>
        public ErrorInfo(string title, string message, ErrorSeverity severity) {
            this.title = title;
            this.message = message;
            this.severity = severity;
        }

        public string Title {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Message {
            get { return this.message; }
            set { this.message = value; }
        }

        public ErrorSeverity Severity {
            get { return this.severity; }
            set { this.severity = value; }
        }

    }
}
