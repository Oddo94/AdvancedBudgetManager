using AdvancedBudgetManagerCore.utils.enums;
using System;

namespace AdvancedBudgetManagerCore.model {
    public class LoginResponse {
        private ResultCode resultCode;
        private string responseMessage;

        public LoginResponse() {
            this.resultCode = ResultCode.UNDEFINED;
            this.responseMessage = String.Empty;
        }

        public LoginResponse(ResultCode resultCode, string responseMessage) {
            this.resultCode = resultCode;
            this.responseMessage = responseMessage;
        }

        public ResultCode ResultCode {
            get { return this.resultCode; }
            set { this.resultCode = value; }
        }

        public string ResponseMessage {
            get { return this.responseMessage; }
            set { this.responseMessage = value; }
        }
    }
}
