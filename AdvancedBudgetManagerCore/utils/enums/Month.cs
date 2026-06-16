using System.ComponentModel;

namespace AdvancedBudgetManagerCore.utils.enums {
    public enum Month {
        [Description("January")]
        January,

        [Description("February")]
        February,

        [Description("March")]
        March,

        [Description("April")]
        April,

        [Description("May")]
        May,

        [Description("June")]
        June,

        [Description("July")]
        July,

        [Description("August")]
        August,

        [Description("September")]
        September,

        [Description("October")]
        October,

        [Description("November")]
        November,

        [Description("December")]
        December,

        [Description("Undefined")]
        Undefined
    }

    static class MonthExtensions {
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

