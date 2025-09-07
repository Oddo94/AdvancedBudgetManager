using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AdvancedBudgetManagerCore.model.message {
    public class GenericRequestMessage : RequestMessage<bool>{
        private string additionalData;

        public GenericRequestMessage() {}

        public GenericRequestMessage(string additionalData) {
            this.additionalData = additionalData;
        }

        public string AdditionalData {
            get { return this.additionalData; }
            set { this.additionalData = value; }
        }
    }
}
