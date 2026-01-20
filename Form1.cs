using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PART2OOPPROJECTUI_DANIELMOURAD
{
    public partial class Form1 : Form
    {
    
        private List<Activity> activities = new List<Activity>();

        private CsvActivityService<FitnessActivity> fitnessService;
        private CsvActivityService<EntertainmentActivity> entertainmentService;

        public Form1()
        {
            InitializeComponent();




            string fitnessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FitnessActivities.csv");
            string entertainmentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EntertainmentActivities.csv");

            fitnessService = new CsvActivityService<FitnessActivity>(fitnessPath, new FitnessActivityMap());
            entertainmentService = new CsvActivityService<EntertainmentActivity>(entertainmentPath, new EntertainmentActivityMap());

        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
        }



        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            bool isEntertainment = radioButton1.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            bool isFitness = radioButton1.Checked;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the click event for searching activities by date.
        /// Filters activities based on Before, On, or After condition.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Use the Search DateTimePicker
                DateTime searchDate = dateTimePicker2.Value.Date;

                // Validate operator selection (Before / On / After)
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Select Before, On, or After.");
                    return;
                }

                string condition = comboBox1.SelectedItem.ToString();
                // Map UI text to stored-proc operator values
                string op =
                    condition.Equals("Before", StringComparison.OrdinalIgnoreCase) ? "before" :
                    condition.Equals("On", StringComparison.OrdinalIgnoreCase) ? "on" :
                    condition.Equals("After", StringComparison.OrdinalIgnoreCase) ? "after" : null;

                if (op is null)
                {
                    MessageBox.Show("Select Before, On, or After.");
                    return;
                }

                // ========================= FILE-BASED (CSV) — PRESERVED FOR REFERENCE =========================
                // activities.Clear();
                // activities.AddRange(fitnessService.LoadActivities());
                // activities.AddRange(entertainmentService.LoadActivities());
                //
                // // Filter activities (CSV path)
                // IEnumerable<Activity> results;
                // if (condition == "Before")
                //     results = activities.Where(a => a.DateStartTime.Date < searchDate.Date);
                // else if (condition == "On")
                //     results = activities.Where(a => a.DateStartTime.Date == searchDate.Date);
                // else // "After"
                //     results = activities.Where(a => a.DateStartTime.Date > searchDate.Date);
                //
                // // Show results or message (CSV path)
                // if (!results.Any())
                // {
                //     MessageBox.Show("No activities found for the selected date.");
                // }
                // dataGridView1.DataSource = null;
                // dataGridView1.DataSource = results.ToList();
                // dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // ========================= DATABASE-BASED — ACTIVE IMPLEMENTATION =============================
                // Call stored proc SearchActivitiesByDate(@SearchDate, @Operator)
                var table = DatabaseHelper.SearchActivitiesByDate(searchDate, op);

                if (table.Rows.Count == 0)
                {
                    MessageBox.Show("No activities found for the selected date.");
                }

                // Bind results to grid
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = table;

                // Simple UI cosmetics
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching activities: " + ex.Message);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = activities; // or results.ToList()
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }





        private void groupBox2_Enter(object sender, EventArgs e)
        {
            groupBox2.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value;
        }


        /// <summary>
        /// Handles the click event for adding a new activity.
        /// Validates input and saves the activity to the CSV file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Read input values from the UI
                DateTime date = dateTimePicker1.Value;
                string title = textBox2.Text.Trim();

                if (!decimal.TryParse(textBox3.Text.Trim(), out decimal cost) || cost < 0)
                {
                    MessageBox.Show("Cost must be a non-negative number.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(title) || title.Length < 3)
                {
                    MessageBox.Show("Title cannot be empty and must have at least 3 characters.");
                    return;
                }

                bool isFitness = radioButton1.Checked;
                bool isEntertainment = radioButton2.Checked;

                if (!isFitness && !isEntertainment)
                {
                    MessageBox.Show("Please select an activity type.");
                    return;
                }

                string location = textBox4.Text.Trim();
                int minParticipants = 0;

                if (isFitness)
                {
                    if (string.IsNullOrWhiteSpace(location))
                    {
                        MessageBox.Show("Location cannot be empty for Fitness Activity.");
                        return;
                    }
                }
                else // Entertainment
                {
                    if (!int.TryParse(textBox5.Text.Trim(), out minParticipants) || minParticipants < 2)
                    {
                        MessageBox.Show("Minimum participants must be at least 2 for Entertainment Activity.");
                        return;
                    }
                }

                // ========================= FILE-BASED (CSV) — PRESERVED FOR REFERENCE =========================
                // if (radioButton1.Checked) // Fitness Activity
                // {
                //     var fitnessActivity = new FitnessActivity(date, title, cost, location);
                //     fitnessService.SaveActivity(fitnessActivity);           // previous CSV/file-based add
                //     MessageBox.Show("Fitness activity added successfully (CSV)!");
                // }
                // else if (radioButton2.Checked) // Entertainment Activity
                // {
                //     var entertainmentActivity = new EntertainmentActivity(date, title, cost, minParticipants);
                //     entertainmentService.SaveActivity(entertainmentActivity); // previous CSV/file-based add
                //     MessageBox.Show("Entertainment activity added successfully (CSV)!");
                // }
                // else
                // {
                //     MessageBox.Show("Please select an activity type.");
                //     return;
                // }
                // RefreshGrid(); // <-- CSV/file-based refresh (preserved but commented)

                // ========================= DATABASE-BASED — ACTIVE IMPLEMENTATION =============================

                // 1) Check if an activity already exists on the selected date (one-per-day rule)
                bool exists = DatabaseHelper.CheckActivityExistsByDate(date);

                if (exists)
                {
                    // Conflict: prompt the user to Replace / Reschedule / Cancel
                    var choice = MessageBox.Show(
                        "An activity is already scheduled on this date.\n\n" +
                        "Yes = Replace existing activity\nNo = Reschedule (pick another date)\nCancel = Abort",
                        "Scheduling Conflict",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (choice == DialogResult.Cancel)
                        return;

                    if (choice == DialogResult.No)
                    {
                        MessageBox.Show("Please choose a different date and try again.");
                        return;
                    }

                    // Replace existing: find existing ActivityID for that date
                    var all = DatabaseHelper.GetAllActivities();
                    var existingRow = all.AsEnumerable()
                                         .FirstOrDefault(r => ((DateTime)r["DateStartTime"]).Date == date.Date);

                    if (existingRow == null)
                    {
                        MessageBox.Show("Could not locate the existing activity to replace. Please try again.");
                        return;
                    }

                    int existingId = Convert.ToInt32(existingRow["ActivityID"]);

                    if (isFitness)
                    {
                        DatabaseHelper.UpdateActivityWithTypeCheck(
                            activityId: existingId,
                            newDateStartTime: date,
                            newTitle: title,
                            newCost: cost,
                            newType: "Fitness",
                            location: location,
                            minParticipants: null);
                    }
                    else // Entertainment
                    {
                        DatabaseHelper.UpdateActivityWithTypeCheck(
                            activityId: existingId,
                            newDateStartTime: date,
                            newTitle: title,
                            newCost: cost,
                            newType: "Entertainment",
                            location: null,
                            minParticipants: minParticipants);
                    }

                    MessageBox.Show("Existing activity replaced.");
                }
                else
                {
                    // No conflict: add new activity via stored procedure
                    if (isFitness)
                    {
                        DatabaseHelper.AddFitnessActivity(date, title, cost, location);
                        MessageBox.Show("Fitness activity added successfully!");
                    }
                    else
                    {
                        DatabaseHelper.AddEntertainmentActivity(date, title, cost, minParticipants);
                        MessageBox.Show("Entertainment activity added successfully!");
                    }
                }

                // 2) Refresh the grid from DATABASE (not CSV)
                // If you created a helper like DisplayAllActivitiesFromDatabase(), call it here.
                // Otherwise, bind GetAllActivities() directly:
                var table = DatabaseHelper.GetAllActivities();
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = table;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding activity: " + ex.Message);
            }
        }


        /// <summary>
        /// Refreshes the DataGridView with all activities from both CSV files.
        /// </summary>

        private void RefreshGrid()
        {
            var fitnessActivities = fitnessService.LoadActivities();
            var entertainmentActivities = entertainmentService.LoadActivities();
            var allActivities = fitnessActivities.Cast<Activity>().Concat(entertainmentActivities).ToList();
            dataGridView1.DataSource = allActivities;


        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {



        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker2.Value;

        }
        private void DisplayAllActivities()
        {
            activities.Clear();
            activities.AddRange(fitnessService.LoadActivities());
            activities.AddRange(entertainmentService.LoadActivities());

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = activities;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button3_Click(object sender, EventArgs e)

        {
            //============== CSV CODE ==============
            //activities.Clear();
            //activities.AddRange(fitnessService.LoadActivities());
            //activities.AddRange(entertainmentService.LoadActivities());

            //dataGridView1.AutoGenerateColumns = true;
            //dataGridView1.DataSource = null;
            //dataGridView1.DataSource = activities;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            //=================== DATABASE BASED IMPLEMENTATION ===================

            var table = DatabaseHelper.GetAllActivities();

            // Bind to grid
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = null;      // optional reset
            dataGridView1.DataSource = table;

            // UI cosmetics 
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            {

                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an activity to delete.");
                    return;
                }

                var selectedRow = dataGridView1.SelectedRows[0];
                var title = selectedRow.Cells["Title"].Value.ToString();
                var date = DateTime.Parse(selectedRow.Cells["DateStartTime"].Value.ToString());

                // Determine type based on which file it belongs to
                bool isFitness = selectedRow.Cells["Location"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["Location"].Value.ToString());

                if (isFitness)
                {
                    fitnessService.DeleteActivity(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && a.DateStartTime == date);
                }
                else
                {
                    entertainmentService.DeleteActivity(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && a.DateStartTime == date);
                }

                RefreshGrid();
                MessageBox.Show("Activity deleted successfully!");
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

    }
}



