using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PART2OOPPROJECTUI_DANIELMOURAD
{




    /// <summary>
    /// Represents a fitness activity with a specific location.
    /// </summary>
    public class FitnessActivity : Activity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FitnessActivity"/> class.
        /// </summary>
        public FitnessActivity() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FitnessActivity"/> class with specified details.
        /// </summary>
        /// <param name="date">The date and time of the activity.</param>
        /// <param name="title">The title of the activity.</param>
        /// <param name="cost">The cost of the activity.</param>
        /// <param name="location">The location where the activity takes place.</param>
        public FitnessActivity(DateTime date, string title, decimal cost, string location)
            : base(date, title, cost)
        {
            Location = location;
            MinParticipants = 0; // Default for fitness
        }

        /// <summary>
        /// Returns details of the fitness activity.
        /// </summary>
        /// <returns>A formatted string with title, location, and date.</returns>
        public override string GetDetails() => $"{Title} at {Location} on {DateStartTime}";


    }
}