using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PART2OOPPROJECTUI_DANIELMOURAD
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using System.Globalization;


    /// <summary>
    /// Provides methods to load and save activities from a CSV file using CsvHelper.
    /// </summary>
    /// <typeparam name="T">The type of activity.</typeparam>

    public class CsvActivityService<T> where T : Activity
    {
        private readonly string filePath;
        private readonly ClassMap<T> map;

        public CsvActivityService(string filePath, ClassMap<T> map)
        {
            this.filePath = filePath;
            this.map = map;
        }

        /// <summary>
        /// Loads all activities from the CSV file.
        /// </summary>
        /// <returns>A list of activities.</returns>

        public List<T> LoadActivities()
        {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                IgnoreBlankLines = true
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap(map);


                csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[]
                  {
                "d/M/yyyy H:mm",
                "dd/MM/yyyy HH:mm",
                "d/M/yyyy H:mm:ss",
                "dd/MM/yyyy HH:mm:ss"
             };


                csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "d/M/yyyy H:mm", "dd/MM/yyyy HH:mm" };
                //csv.Context.MissingFieldFound = null; // Ignore missing fields
                //csv.Configuration.HeaderValidated = null; // Ignore header validation
                return csv.GetRecords<T>().ToList();
            }
        }


        /// <summary>
        /// Saves a new activity to the CSV file.
        /// </summary>
        /// <param name="activity">The activity to save.</param>

        public void SaveActivity(T activity)
        {
            var activities = LoadActivities(); // Load existing records
            activities.Add(activity); // Add new one

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap(map);
                csv.WriteHeader<T>();
                csv.NextRecord();
                csv.WriteRecords(activities);
            }
        }


        /// <summary>
        /// Deletes an activity from the CSV file based on a condition.
        /// </summary>
        /// <param name="predicate">The condition to match the activity to delete.</param>

        public void DeleteActivity(Func<T, bool> predicate)
        {
            var activities = LoadActivities();
            var toRemove = activities.FirstOrDefault(predicate);
            if (toRemove != null)
            {
                activities.Remove(toRemove);
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap(map);
                    csv.WriteHeader<T>();
                    csv.NextRecord();
                    csv.WriteRecords(activities);
                }
            }
        }



        public void AddActivity(T activity)
        {
            var activities = LoadActivities();
            activities.Add(activity);

            using (var reader = new StreamReader(filePath))
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {

                if (typeof(T) == typeof(FitnessActivity))
                    csv.Context.RegisterClassMap<FitnessActivityMap>();
                else if (typeof(T) == typeof(EntertainmentActivity))
                    csv.Context.RegisterClassMap<EntertainmentActivityMap>();

                csv.WriteRecords(activities);


            }

        }
    }
}