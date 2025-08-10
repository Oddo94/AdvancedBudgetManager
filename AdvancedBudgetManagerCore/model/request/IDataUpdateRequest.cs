using AdvancedBudgetManagerCore.utils.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.request {
    public interface IDataUpdateRequest {
        DataUpdateRequestType GetDataUpdateRequestType();

        string GetUpdateParameter();
    }
}
