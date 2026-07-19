using AdvancedBudgetManager.utils.enums;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace AdvancedBudgetManager.utils.misc {
    [MarkupExtensionReturnType(ReturnType = typeof(IconElement))]
    public class AppIconExtension : MarkupExtension {
        public AppIcon AppIcon { get; set; }

        protected override object ProvideValue() {
            string fileName = AppIcon.ToString().ToLower();

            return new ImageIcon {
                Source = new BitmapImage(new Uri($"ms-appx:///Assets/Icons/{fileName}.png"))
            };
        }


    }
}
