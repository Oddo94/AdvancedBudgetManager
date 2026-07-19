using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    /// <summary>
    /// Enum class that contains the accepted list of values which can be used to specify the month of the year.
    /// </summary>
    public enum Month {
        /// <summary>
        /// Value for January.
        /// </summary>
        [Description("January")]
        January,

        /// <summary>
        /// Value for February.
        /// </summary>
        [Description("February")]
        February,

        /// <summary>
        /// Value for March.
        /// </summary>
        [Description("March")]
        March,

        /// <summary>
        /// Value for April.
        /// </summary>
        [Description("April")]
        April,

        /// <summary>
        /// Value for May.
        /// </summary>
        [Description("May")]
        May,

        /// <summary>
        /// Value for June.
        /// </summary>
        [Description("June")]
        June,

        /// <summary>
        /// Value for July.
        /// </summary>
        [Description("July")]
        July,

        /// <summary>
        /// Value for August.
        /// </summary>
        [Description("August")]
        August,

        /// <summary>
        /// Value for September.
        /// </summary>
        [Description("September")]
        September,

        /// <summary>
        /// Value for October.
        /// </summary>
        [Description("October")]
        October,

        /// <summary>
        /// Value for November.
        /// </summary>
        [Description("November")]
        November,

        /// <summary>
        /// Value for December.
        /// </summary>
        [Description("December")]
        December,

        /// <summary>
        /// The default value to be used when the month is unknown.
        /// </summary>
        [Description("Undefined")]
        Undefined
    }

    /// <summary>
    /// Extension class used for providing utility methods related to the <see cref="Month"/> enum class.
    /// </summary>
    static class MonthExtensions {
        /// <summary>
        /// Returns the corresponding <see cref="Month"/> enum value based on its description.
        /// </summary>
        /// <param name="enumDescription">The month description as a <see cref="string"/> object.</param>
        /// <returns>A <see cref="Month"/> enum value.</returns>
        public static Month GetTypeByDescription(string enumDescription) {
            Month month;
            switch (enumDescription) {
                case "January":
                    month = Month.January;
                    break;

                case "February":
                    month = Month.February;
                    break;

                case "March":
                    month = Month.March;
                    break;

                case "April":
                    month = Month.April;
                    break;

                case "May":
                    month = Month.May;
                    break;

                case "June":
                    month = Month.June;
                    break;

                case "July":
                    month = Month.July;
                    break;

                case "August":
                    month = Month.August;
                    break;

                case "September":
                    month = Month.September;
                    break;

                case "October":
                    month = Month.October;
                    break;

                case "November":
                    month = Month.November;
                    break;

                case "December":
                    month = Month.December;
                    break;

                default:
                    month = Month.Undefined;
                    break;
            }

            return month;
        }
    }

}

