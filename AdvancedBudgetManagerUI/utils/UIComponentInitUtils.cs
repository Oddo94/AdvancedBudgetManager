using AdvancedBudgetManagerCore.utils.enums;
using System;
using System.Collections.Generic;

namespace AdvancedBudgetManager.utils {
    public class UIComponentInitUtils {
        public UIComponentInitUtils() { }

        public List<string> InitColumnChartLabels(TimeUnit timeUnit) {
            List<string> defaultLabels = new List<string>();

            switch (timeUnit) {
                case TimeUnit.Day:
                    int daysInMonth = 30;
                    for (int i = 1; i <= daysInMonth; i++) {
                        defaultLabels.Add(Convert.ToString(i));
                    }
                    break;

                case TimeUnit.Month:
                    defaultLabels = new List<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    break;

                case TimeUnit.Undefined:
                    break;

            }

            return defaultLabels;
        }
    }
}
