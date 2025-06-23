using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    public enum DataRequestType {
        [Description ("LOGIN_DATA_RETRIEVAL")]
        LOGIN_DATA_RETRIEVAL,
        [Description ("BUDGET_SUMMARY_DATA_RETRIEVAL")]
        BUDGET_SUMMARY_DATA_RETRIEVAL,
        [Description ("UNDEFINED")]
        UNDEFINED
    }
}
