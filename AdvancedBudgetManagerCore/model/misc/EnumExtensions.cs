using System;
using System.ComponentModel;
using System.Reflection;

namespace AdvancedBudgetManagerCore.model.misc {
    /// <summary>
    /// Extension class used for providing utility methods for enum classes.
    /// </summary>
    public static class EnumExtensions {
        /// <summary>
        /// Retrieves the description of the provided enum.
        /// </summary>
        /// <param name="enumValue">The enum value for which the description should be retrieved.</param>
        /// <returns>The enum description.</returns>
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
