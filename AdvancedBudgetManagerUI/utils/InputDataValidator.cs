using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdvancedBudgetManager.utils {
    /// <summary>
    /// Utility class that contains methods for validating user input.
    /// </summary>
    public class InputDataValidator {

        /// <summary>
        /// Checks if the input password contains all the required character types (lowercase, uppercase, digits, special characters). 
        /// </summary>
        /// <param name="password">The input password.</param>
        /// <returns>A <see cref="bool"> value indicating if the password is valid./></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool IsValidPassword([DisallowNull] String password) {
            if (password == null) {
                throw new ArgumentException("The input password cannot be null!");
            }

            //Checks if the password contains lowercase, uppercase characters and digits
            Regex firstRegexPattern = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\d]).+$", RegexOptions.Compiled);
            //Checks if the password contains special characters
            Regex secondRegexPattern = new Regex(".*[!@#\\$%^&*()_\\+\\-\\=\\[\\[{};'\\:\"\\|,.\\/<>\\?`~]+.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);


            MatchCollection match1 = firstRegexPattern.Matches(password);
            MatchCollection match2 = secondRegexPattern.Matches(password);


            if (firstRegexPattern.IsMatch(password) && secondRegexPattern.IsMatch(password)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the input string has the required length. The evaluation is based on the required length and the chosen <see cref="ComparisonMode"/>.
        /// </summary>
        /// <param name="input">The <see cref="String"/> object to be checked.</param>
        /// <param name="requiredLength">The specified required length.</param>
        /// <param name="comparisonMode">The <see cref="ComparisonModeMode"/> enum which indicated how the comparison should be performed.</param>
        /// <returns>A <see cref="bool"> value indicating if the password has the required length.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool HasRequiredLength(String input, int requiredLength, ComparisonMode comparisonMode) {
            if (input == null) {
                throw new ArgumentException("The input string cannot be null!");
            }

            if (requiredLength < 0) {
                throw new ArgumentException("The minimum size must be greater than or equal to 0!");
            }

            if (comparisonMode == ComparisonMode.LENIENT) {
                if (input.Length >= requiredLength) {
                    return true;
                }
            } else if (comparisonMode == ComparisonMode.STRICT) {
                if (input.Length == requiredLength) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the provided input is null.
        /// </summary>
        /// <param name="input">The <see cref="String"/> object to be checked.</param>
        /// <returns>A <see cref="bool"> value indicating if the password is null.</returns>
        public bool IsNull(String input) {
            return input == null;
        }

        /// <summary>
        /// Checks if the provided parameters match.
        /// </summary>
        /// <param name="firstParameter">The value that needs to be compared.</param>
        /// <param name="secondParameter">The value against which the comparison is performed.</param>
        /// <returns>A <see cref="bool"> value indicating if the input values match.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool IsMatch(String firstParameter, String secondParameter) {
            if (firstParameter == null) {
                throw new ArgumentException("The first parameter cannot be null!");
            }

            return firstParameter.Equals(secondParameter);
        }

        /// <summary>
        /// Checks if the provided input is empty.
        /// </summary>
        /// <param name="input">The <see cref="String"/> object to be checked.</param>
        /// <returns>A <see cref="bool"> value indicating if the input is empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool IsEmpty(String input) {
            if (input == null) {
                throw new ArgumentException("The input cannot be null!");
            }

            return input.Equals("");
        }
    }
}
