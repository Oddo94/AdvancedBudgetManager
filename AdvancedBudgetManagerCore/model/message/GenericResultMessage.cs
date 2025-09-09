using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.message {
    public class GenericResultMessage {
        private bool isSuccess;
        private string message;

        public GenericResultMessage() { }

        public GenericResultMessage(bool isSuccess, string message) {
            this.isSuccess = isSuccess;
            this.message = message;
        }

        public bool IsSuccess {
            get { return this.isSuccess; }
            set { this.isSuccess = value; }
        }

        public string Message {
            get { return this.message; }
            set { this.message = value; }
        }
    }
}
