using AdvancedBudgetManagerCore.utils.enums;
using System;

namespace AdvancedBudgetManagerCore.model {
    /// <summary>
    /// Represents the object that contains the result of the login process.
    /// </summary>
    public class LoginResponse {
        private ResultCode resultCode;
        private string responseMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginResponse"/> class with no arguments.
        /// </summary>
        public LoginResponse() {
            this.resultCode = ResultCode.UNDEFINED;
            this.responseMessage = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginResponse"/> class based on a <see cref="ResultCode"/> and <see cref="string"/> response message.
        /// </summary>
        /// <param name="resultCode">The result code of the login operation.</param>
        /// <param name="responseMessage">The response message of the login operation.</param>
        public LoginResponse(ResultCode resultCode, string responseMessage) {
            this.resultCode = resultCode;
            this.responseMessage = responseMessage;
        }

        /// <summary>
        /// Sets/retrieves the result code of the <see cref="LoginResponse"/>.
        /// </summary>
        public ResultCode ResultCode {
            get { return this.resultCode; }
            set { this.resultCode = value; }
        }

        /// <summary>
        /// Sets/retrieves the response message of the <see cref="LoginResponse"/>.
        /// </summary>
        public string ResponseMessage {
            get { return this.responseMessage; }
            set { this.responseMessage = value; }
        }
    }
}
