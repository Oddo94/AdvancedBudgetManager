using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.response {
    /// <summary>
    /// Represents the object that contains the result of the login process.
    /// </summary>
    public class GenericResponse {
        private ResultCode resultCode;
        private string responseMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResponse"/> class with no arguments.
        /// </summary>
        public GenericResponse() {
            resultCode = ResultCode.Undefined;
            responseMessage = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResponse"/> class based on a <see cref="ResultCode"/> and <see cref="string"/> response message.
        /// </summary>
        /// <param name="resultCode">The result code of the login operation.</param>
        /// <param name="responseMessage">The response message of the login operation.</param>
        public GenericResponse(ResultCode resultCode, string responseMessage) {
            this.resultCode = resultCode;
            this.responseMessage = responseMessage;
        }

        /// <summary>
        /// Sets/retrieves the result code of the <see cref="GenericResponse"/>.
        /// </summary>
        public ResultCode ResultCode {
            get { return resultCode; }
            set { resultCode = value; }
        }

        /// <summary>
        /// Sets/retrieves the response message of the <see cref="GenericResponse"/>.
        /// </summary>
        public string ResponseMessage {
            get { return responseMessage; }
            set { responseMessage = value; }
        }
    }
}
