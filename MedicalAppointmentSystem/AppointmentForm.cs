using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;

namespace MedicalAppointmentSystem
{
    public partial class AppointmentForm : Form
    {
        private ComboBox cboDoctors;
        private ComboBox cboPatients;
        private DateTimePicker dtpAppointmentDate;
        private TextBox txtNotes;
        private Button btnBook;
        private Button btnClear;
        private Button btnClose;
        private Label lblTitle;
        private Label lblDoctor;
        private Label lblPatient;
        private Label lblDate;
        private Label lblNotes;

        public AppointmentForm()
        {
            InitializeComponent();
            LoadDoctors();
            LoadPatients();
        }

        private void InitializeComponent()
        {
            this.Text = "Book New Appointment";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Create main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Create title label
            lblTitle = new Label
            {
                Text = "Book New Appointment",
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Create form panel
            Panel formPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Create labels and controls
            lblDoctor = new Label
            {
                Text = "Select Doctor:",
                Location = new Point(20, 20),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10)
            };

            cboDoctors = new ComboBox
            {
                Location = new Point(150, 20),
                Size = new Size(300, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };

            lblPatient = new Label
            {
                Text = "Select Patient:",
                Location = new Point(20, 60),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10)
            };

            cboPatients = new ComboBox
            {
                Location = new Point(150, 60),
                Size = new Size(300, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };

            lblDate = new Label
            {
                Text = "Appointment Date:",
                Location = new Point(20, 100),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10)
            };

            dtpAppointmentDate = new DateTimePicker
            {
                Location = new Point(150, 100),
                Size = new Size(300, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy hh:mm tt",
                ShowUpDown = true,
                Font = new Font("Arial", 10)
            };

            lblNotes = new Label
            {
                Text = "Notes:",
                Location = new Point(20, 140),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10)
            };

            txtNotes = new TextBox
            {
                Location = new Point(150, 140),
                Size = new Size(300, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Arial", 10)
            };

            // Create button panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(10)
            };

            // Create buttons
            btnBook = new Button
            {
                Text = "Book Appointment",
                Size = new Size(120, 40),
                Location = new Point(10, 10),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnBook.Click += BtnBook_Click;

            btnClear = new Button
            {
                Text = "Clear",
                Size = new Size(80, 40),
                Location = new Point(140, 10)
            };
            btnClear.Click += BtnClear_Click;

            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(80, 40),
                Location = new Point(230, 10)
            };
            btnClose.Click += (sender, e) => this.Close();

            // Add controls to form panel
            formPanel.Controls.Add(lblDoctor);
            formPanel.Controls.Add(cboDoctors);
            formPanel.Controls.Add(lblPatient);
            formPanel.Controls.Add(cboPatients);
            formPanel.Controls.Add(lblDate);
            formPanel.Controls.Add(dtpAppointmentDate);
            formPanel.Controls.Add(lblNotes);
            formPanel.Controls.Add(txtNotes);

            // Add controls to button panel
            buttonPanel.Controls.Add(btnBook);
            buttonPanel.Controls.Add(btnClear);
            buttonPanel.Controls.Add(btnClose);

            // Add panels to main panel
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(formPanel);
            mainPanel.Controls.Add(buttonPanel);

            this.Controls.Add(mainPanel);
        }

        private void LoadDoctors()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT DoctorID, FullName + ' - ' + Specialty AS DoctorInfo FROM Doctors WHERE Availability = 1 ORDER BY FullName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            cboDoctors.Items.Clear();
                            while (reader.Read())
                            {
                                cboDoctors.Items.Add(new DoctorItem
                                {
                                    DoctorID = reader.GetInt32("DoctorID"),
                                    DisplayText = reader.GetString("DoctorInfo")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPatients()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT PatientID, FullName + ' (' + Email + ')' AS PatientInfo FROM Patients ORDER BY FullName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            cboPatients.Items.Clear();
                            while (reader.Read())
                            {
                                cboPatients.Items.Add(new PatientItem
                                {
                                    PatientID = reader.GetInt32("PatientID"),
                                    DisplayText = reader.GetString("PatientInfo")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patients: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBook_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                BookAppointment();
            }
        }

        private bool ValidateForm()
        {
            if (cboDoctors.SelectedItem == null)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboPatients.SelectedItem == null)
            {
                MessageBox.Show("Please select a patient.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpAppointmentDate.Value <= DateTime.Now)
            {
                MessageBox.Show("Please select a future date and time for the appointment.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BookAppointment()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) 
                                   VALUES (@DoctorID, @PatientID, @AppointmentDate, @Notes)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters
                        command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = 
                            ((DoctorItem)cboDoctors.SelectedItem).DoctorID;
                        command.Parameters.Add("@PatientID", SqlDbType.Int).Value = 
                            ((PatientItem)cboPatients.SelectedItem).PatientID;
                        command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = 
                            dtpAppointmentDate.Value;
                        command.Parameters.Add("@Notes", SqlDbType.VarChar, 500).Value = 
                            txtNotes.Text.Trim();

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Appointment booked successfully!", "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error booking appointment: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            cboDoctors.SelectedIndex = -1;
            cboPatients.SelectedIndex = -1;
            dtpAppointmentDate.Value = DateTime.Now.AddDays(1);
            txtNotes.Clear();
        }

        // Helper classes for ComboBox items
        private class DoctorItem
        {
            public int DoctorID { get; set; }
            public string DisplayText { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        private class PatientItem
        {
            public int PatientID { get; set; }
            public string DisplayText { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }
    }
} 