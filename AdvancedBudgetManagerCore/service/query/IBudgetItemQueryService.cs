using System;
using System.Collections.Generic;

namespace AdvancedBudgetManagerCore.service.query {
    public interface IBudgetItemQueryService<TDto, TKey> {
        public List<TDto> GetAllByIdAndDateInterval(long userId);

        public TDto GetById(TKey id);

        public TDto GetAggregatedDataByType(long userId, DateTime startDate, DateTime endDate);

        public TDto GetMonthlyAggregatedDataForYear(long userId, int year);
    }
}
