using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.message {
    #pragma warning disable CS1591
    /// <summary>
    /// Represents the object that stores the result that is received after performing a generic action.
    /// </summary>
    public class GenericResultMessage {
        /// <summary>
        /// Flag that indicates the succes/failure of the action.
        /// </summary>
        private bool isSuccess;

        /// <summary>
        /// The actual message that is received after performing the action.
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResultMessage"/> with default values.
        /// </summary>
        public GenericResultMessage() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResultMessage"/> based on the boolean flag indicating the succes/failure of the operation and the message.
        /// </summary>
        /// <param name="isSuccess">A flag that indicates the success/failure of the operation</param>
        /// <param name="message">The message received after performing the action</param>
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
