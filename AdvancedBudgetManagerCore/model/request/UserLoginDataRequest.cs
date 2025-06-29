using AdvancedBudgetManagerCore.utils.enums;

namespace AdvancedBudgetManagerCore.model.request {
    /// <summary>
    /// Represents the request for performing the user login process which contains the required data.
    /// </summary>
    public class UserLoginDataRequest : IDataRequest {
        private string userName;

        private string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginDataRequest"/> based on the username and password>
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        public UserLoginDataRequest(string userName, string password) {
            this.userName = userName;
            this.password = password;
        }

        /// <summary>
        /// Provides the actual type of the data request
        /// </summary>
        /// <returns>A <see cref="DataRequestType"/> containing the request type</returns>
        public DataRequestType GetDataRequestType() {
            return DataRequestType.LOGIN_DATA_RETRIEVAL;
        }

        /// <summary>
        /// Retrieves the parameter value which will be used for querying the data.
        /// </summary>
        /// <returns></returns>
        public string GetSearchParameter() {
            return this.userName;
        }
    }
}
