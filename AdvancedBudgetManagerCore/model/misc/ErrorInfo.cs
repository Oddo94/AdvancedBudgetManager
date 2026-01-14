using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.misc {
    public class ErrorInfo {
        private string title;
        private string message;
        private ErrorSeverity severity;

        public ErrorInfo() { }

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
