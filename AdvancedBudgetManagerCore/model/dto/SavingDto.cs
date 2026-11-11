using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManagerCore.model.dto {
#pragma warning disable CS1591
    /// <summary>
    /// Represents the data transfer object used for storing the information related to the user's savings.
    /// </summary>
    public class SavingDto {
        /// <summary>
        /// The saving name.
        /// </summary>
        private string name;


        /// <summary>
        /// The saving value.
        /// </summary>
        private long value;

        /// <summary>
        /// The saving date.
        /// </summary>
        private DateTime date;


        /// <summary>
        /// Initializes a new instance of the <see cref="SavingDto"/> based on the provided arguments.
        /// </summary>
        /// <param name="name">The saving name.</param>
        /// <param name="value">The saving value.</param>
        /// <param name="date">The saving date.</param>
        public SavingDto(string name, long value, DateTime date) {
            this.name = name;
            this.value = value;
            this.date = date;
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public long Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public DateTime Date {
            get { return this.date; }
            set { this.date = value; }
        }
    }
}
