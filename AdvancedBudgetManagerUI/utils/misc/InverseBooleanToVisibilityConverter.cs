using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace AdvancedBudgetManager.utils.misc {
    public class InverseBooleanToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value is bool b ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return value is Visibility v && v == Visibility.Visible;
        }
    }
}
