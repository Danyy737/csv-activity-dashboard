using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PART2OOPPROJECTUI_DANIELMOURAD
{

    /// <summary>
    /// Represents a base activity with common properties such as date, title, cost, location, and minimum participants.
    /// </summary>

    public abstract class Activity
    {

        /// <summary>
        /// Gets or sets the start date and time of the activity.
        /// </summary>

        public DateTime DateStartTime { get; set; }

        /// <summary>
        /// Gets or sets the title of the activity.
        /// </summary>

        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the cost of the activity.
        /// </summary>

        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the location of the activity.
        /// </summary>

        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of participants required for the activity.
        /// </summary>

        public int MinParticipants { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity"/> class. Required for CsvHelper.
        /// </summary>

        public Activity() { } // Needed for CsvHelper

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity"/> class with all properties.
        /// </summary>
        /// <param name="date">The start date and time of the activity.</param>
        /// <param name="title">The title of the activity.</param>
        /// <param name="cost">The cost of the activity.</param>
        /// <param name="location">The location of the activity.</param>
        /// <param name="minParticipants">The minimum number of participants required.</param>

        public Activity(DateTime date, string title, decimal cost, string location, int minParticipants)
        {
            DateStartTime = date;
            Title = title;
            Cost = cost;
            Location = location;
            MinParticipants = minParticipants;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity"/> class with basic properties.
        /// </summary>
        /// <param name="date">The start date and time of the activity.</param>
        /// <param name="title">The title of the activity.</param>
        /// <param name="cost">The cost of the activity.</param>

        protected Activity(DateTime date, string title, decimal cost)
        {
            DateStartTime = date;
            Title = title;
            Cost = cost;
        }

        /// <summary>
        /// Returns a string containing details of the activity.
        /// </summary>
        /// <returns>A formatted string describing the activity.</returns>

        public abstract string GetDetails();
    }

}