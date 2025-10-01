using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdvancedBudgetManager.utils {
    public class InputDataValidator {


        public bool IsValidPassword([DisallowNull] String password) {
            if (password == null) {
                throw new ArgumentException("The input password cannot be null!");
            }

            //Checks if the password contains lowercase, uppercase characters and digits
            Regex firstRegexPattern = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\d]).+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //Checks if the password contains special characters
            Regex secondRegexPattern = new Regex(".*[!@#\\$%^&*()_\\+\\-\\=\\[\\[{};'\\:\"\\|,.\\/<>\\?`~]+.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);


            MatchCollection match1 = firstRegexPattern.Matches(password);
            MatchCollection match2 = secondRegexPattern.Matches(password);


            if (firstRegexPattern.IsMatch(password) && secondRegexPattern.IsMatch(password)) {
                return true;
            }

            return false;
        }
    }
}
