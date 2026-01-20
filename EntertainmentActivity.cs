using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PART2OOPPROJECTUI_DANIELMOURAD
{



    /// <summary>
    /// Represents an entertainment activity with minimum participants.
    /// </summary>
    public class EntertainmentActivity : Activity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntertainmentActivity"/> class.
        /// </summary>
        public EntertainmentActivity() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntertainmentActivity"/> class with specified details.
        /// </summary>
        /// <param name="date">The date and time of the activity.</param>
        /// <param name="title">The title of the activity.</param>
        /// <param name="cost">The cost of the activity.</param>
        /// <param name="minParticipants">The minimum number of participants required.</param>
        public EntertainmentActivity(DateTime date, string title, decimal cost, int minParticipants)
            : base(date, title, cost)
        {
            MinParticipants = minParticipants;
            Location = "No Location for Entertainment";
        }

        /// <summary>
        /// Returns details of the entertainment activity.
        /// </summary>
        /// <returns>A formatted string with title, participants, and date.</returns>
        public override string GetDetails() => $"{Title} with {MinParticipants}+ participants on {DateStartTime}";
    }

}
