using AdvancedBudgetManagerCore.utils.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.request {
    public class UserLoginDataRequest : IDataRequest {
        private string userName;

        private string password;

        public UserLoginDataRequest(string userName, string password) {
            this.userName = userName;
            this.password = password;
        }

        public DataRequestType GetDataRequestType() {
            return DataRequestType.LOGIN_DATA_RETRIEVAL;
        }

        public string GetSearchParameter() {
            return this.userName;
        }
    }
}
