using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AdvancedBudgetManagerCore.model.message {
    #pragma warning disable CS1591
    /// <summary>
    /// Represents the object that stores the result that is received after submitting a generic request.
    /// </summary>
    public class GenericRequestMessage : RequestMessage<bool>{
        /// <summary>
        /// The request data.
        /// </summary>
        private string additionalData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRequestMessage"/> with default values.
        /// </summary>
        public GenericRequestMessage() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRequestMessage"/> based on the request data.
        /// </summary>
        /// <param name="additionalData">The request data</param>
        public GenericRequestMessage(string additionalData) {
            this.additionalData = additionalData;
        }

        public string AdditionalData {
            get { return this.additionalData; }
            set { this.additionalData = value; }
        }
    }
}
