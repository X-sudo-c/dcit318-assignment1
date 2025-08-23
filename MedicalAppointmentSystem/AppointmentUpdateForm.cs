using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;

namespace MedicalAppointmentSystem
{
    public partial class AppointmentUpdateForm : Form
    {
        private DateTimePicker dtpAppointmentDate;
        private TextBox txtNotes;
        private Button btnUpdate;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblDate;
        private Label lblNotes;

        private int appointmentID;
        private DateTime originalDate;
        private string originalNotes;

        public AppointmentUpdateForm(int appointmentID, DateTime currentDate, string currentNotes)
        {
            this.appointmentID = appointmentID;
            this.originalDate = currentDate;
            this.originalNotes = currentNotes;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Update Appointment";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Create title label
            lblTitle = new Label
            {
                Text = "Update Appointment Details",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };

            // Create form panel
            Panel formPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            // Create labels and controls
            lblDate = new Label
            {
                Text = "Appointment Date:",
                Location = new Point(20, 20),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10)
            };

            dtpAppointmentDate = new DateTimePicker
            {
                Location = new Point(150, 20),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy hh:mm tt",
                ShowUpDown = true,
                Font = new Font("Arial", 10),
                Value = originalDate
            };

            lblNotes = new Label
            {
                Text = "Notes:",
                Location = new Point(20, 60),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10)
            };

            txtNotes = new TextBox
            {
                Location = new Point(150, 60),
                Size = new Size(200, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Arial", 10),
                Text = originalNotes
            };

            // Create button panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(10)
            };

            // Create buttons
            btnUpdate = new Button
            {
                Text = "Update",
                Size = new Size(80, 35),
                Location = new Point(10, 10),
                BackColor = Color.LightBlue,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(80, 35),
                Location = new Point(100, 10)
            };
            btnCancel.Click += (sender, e) => this.DialogResult = DialogResult.Cancel;

            // Add controls to form panel
            formPanel.Controls.Add(lblDate);
            formPanel.Controls.Add(dtpAppointmentDate);
            formPanel.Controls.Add(lblNotes);
            formPanel.Controls.Add(txtNotes);

            // Add controls to button panel
            buttonPanel.Controls.Add(btnUpdate);
            buttonPanel.Controls.Add(btnCancel);

            // Add panels to main panel
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(formPanel);
            mainPanel.Controls.Add(buttonPanel);

            this.Controls.Add(mainPanel);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                UpdateAppointment();
            }
        }

        private bool ValidateForm()
        {
            if (dtpAppointmentDate.Value <= DateTime.Now)
            {
                MessageBox.Show("Please select a future date and time for the appointment.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void UpdateAppointment()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"UPDATE Appointments 
                                   SET AppointmentDate = @AppointmentDate, Notes = @Notes 
                                   WHERE AppointmentID = @AppointmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters
                        command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
                        command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = dtpAppointmentDate.Value;
                        command.Parameters.Add("@Notes", SqlDbType.VarChar, 500).Value = txtNotes.Text.Trim();

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No appointment was updated. Please check the appointment ID.", "Warning", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 