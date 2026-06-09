using Microsoft.UI.Xaml.Data;
using System;

namespace AdvancedBudgetManager.utils.misc {
    public class DateTimeFormatConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is DateTime date) {
                return date.ToString("yyyy-MM-dd");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
