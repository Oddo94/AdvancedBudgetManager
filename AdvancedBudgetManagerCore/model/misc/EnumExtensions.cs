using System;
using System.ComponentModel;
using System.Reflection;

namespace AdvancedBudgetManagerCore.model.misc {
    public static class EnumExtensions {

        public static string GetEnumDescription(Enum enumValue) {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            string enumDescription = string.Empty;
            if (descriptionAttributes.Length > 0) {
                enumDescription = descriptionAttributes[0].Description;
            } else {
                enumDescription = enumValue.ToString();
            }

            return enumDescription;
        }
    }
}
