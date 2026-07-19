using System;

namespace AdvancedBudgetManager.utils.misc {
    public class CustomEventArgs : EventArgs {
        private string message;

        public CustomEventArgs(string message) {
            this.message = message;
        }

        public string Message {
            get { return this.message; }
            set { this.message = value; }
        }
    }
}
