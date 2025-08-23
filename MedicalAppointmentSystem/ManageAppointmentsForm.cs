using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;

namespace MedicalAppointmentSystem
{
    public partial class ManageAppointmentsForm : Form
    {
        private DataGridView dgvAppointments;
        private Button btnRefresh;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClose;
        private Label lblTitle;
        private ComboBox cboFilterPatient;
        private Label lblFilterPatient;
        private DataSet appointmentsDataSet;
        private SqlDataAdapter dataAdapter;

        public ManageAppointmentsForm()
        {
            InitializeComponent();
            LoadAppointments();
        }

        private void InitializeComponent()
        {
            this.Text = "Manage Appointments";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Create title label
            lblTitle = new Label
            {
                Text = "Manage Appointments",
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Create filter panel
            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10)
            };

            lblFilterPatient = new Label
            {
                Text = "Filter by Patient:",
                Location = new Point(10, 20),
                Size = new Size(100, 25),
                Font = new Font("Arial", 10)
            };

            cboFilterPatient = new ComboBox
            {
                Location = new Point(120, 20),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };
            cboFilterPatient.SelectedIndexChanged += CboFilterPatient_SelectedIndexChanged;

            Button btnLoadPatients = new Button
            {
                Text = "Load Patients",
                Location = new Point(330, 20),
                Size = new Size(100, 25),
                Font = new Font("Arial", 9)
            };
            btnLoadPatients.Click += BtnLoadPatients_Click;

            filterPanel.Controls.Add(lblFilterPatient);
            filterPanel.Controls.Add(cboFilterPatient);
            filterPanel.Controls.Add(btnLoadPatients);

            // Create DataGridView
            dgvAppointments = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                MultiSelect = false
            };

            // Create button panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(10)
            };

            // Create buttons
            btnRefresh = new Button
            {
                Text = "Refresh",
                Size = new Size(100, 40),
                Location = new Point(10, 10)
            };
            btnRefresh.Click += BtnRefresh_Click;

            btnUpdate = new Button
            {
                Text = "Update Selected",
                Size = new Size(120, 40),
                Location = new Point(120, 10),
                BackColor = Color.LightBlue
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button
            {
                Text = "Delete Selected",
                Size = new Size(120, 40),
                Location = new Point(250, 10),
                BackColor = Color.LightCoral
            };
            btnDelete.Click += BtnDelete_Click;

            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 40),
                Location = new Point(380, 10)
            };
            btnClose.Click += (sender, e) => this.Close();

            // Add controls to button panel
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Controls.Add(btnUpdate);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnClose);

            // Add panels to main panel
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(filterPanel);
            mainPanel.Controls.Add(dgvAppointments);
            mainPanel.Controls.Add(buttonPanel);

            this.Controls.Add(mainPanel);
        }

        private void LoadPatients()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT PatientID, FullName FROM Patients ORDER BY FullName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            cboFilterPatient.Items.Clear();
                            cboFilterPatient.Items.Add("All Patients");
                            while (reader.Read())
                            {
                                cboFilterPatient.Items.Add(new PatientFilterItem
                                {
                                    PatientID = reader.GetInt32("PatientID"),
                                    PatientName = reader.GetString("FullName")
                                });
                            }
                            cboFilterPatient.SelectedIndex = 0;
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

        private void LoadAppointments()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT a.AppointmentID, a.DoctorID, a.PatientID, 
                                   a.AppointmentDate, a.Notes,
                                   d.FullName AS DoctorName, d.Specialty,
                                   p.FullName AS PatientName, p.Email
                                   FROM Appointments a
                                   INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
                                   INNER JOIN Patients p ON a.PatientID = p.PatientID
                                   ORDER BY a.AppointmentDate DESC";

                    dataAdapter = new SqlDataAdapter(query, connection);
                    appointmentsDataSet = new DataSet();
                    dataAdapter.Fill(appointmentsDataSet, "Appointments");

                    dgvAppointments.DataSource = appointmentsDataSet.Tables["Appointments"];

                    // Format columns
                    if (dgvAppointments.Columns.Count > 0)
                    {
                        dgvAppointments.Columns["AppointmentID"].HeaderText = "ID";
                        dgvAppointments.Columns["DoctorID"].Visible = false;
                        dgvAppointments.Columns["PatientID"].Visible = false;
                        dgvAppointments.Columns["AppointmentDate"].HeaderText = "Date & Time";
                        dgvAppointments.Columns["Notes"].HeaderText = "Notes";
                        dgvAppointments.Columns["DoctorName"].HeaderText = "Doctor";
                        dgvAppointments.Columns["Specialty"].HeaderText = "Specialty";
                        dgvAppointments.Columns["PatientName"].HeaderText = "Patient";
                        dgvAppointments.Columns["Email"].HeaderText = "Patient Email";

                        // Set column widths
                        dgvAppointments.Columns["AppointmentID"].Width = 50;
                        dgvAppointments.Columns["AppointmentDate"].Width = 150;
                        dgvAppointments.Columns["Notes"].Width = 200;
                        dgvAppointments.Columns["DoctorName"].Width = 150;
                        dgvAppointments.Columns["Specialty"].Width = 120;
                        dgvAppointments.Columns["PatientName"].Width = 150;
                        dgvAppointments.Columns["Email"].Width = 200;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLoadPatients_Click(object sender, EventArgs e)
        {
            LoadPatients();
        }

        private void CboFilterPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFilterPatient.SelectedItem is PatientFilterItem patientItem)
            {
                FilterAppointmentsByPatient(patientItem.PatientID);
            }
            else if (cboFilterPatient.SelectedIndex == 0)
            {
                LoadAppointments(); // Show all appointments
            }
        }

        private void FilterAppointmentsByPatient(int patientID)
        {
            try
            {
                if (appointmentsDataSet != null && appointmentsDataSet.Tables.Count > 0)
                {
                    DataView dv = appointmentsDataSet.Tables["Appointments"].DefaultView;
                    dv.RowFilter = $"PatientID = {patientID}";
                    dgvAppointments.DataSource = dv;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering appointments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                UpdateSelectedAppointment();
            }
            else
            {
                MessageBox.Show("Please select an appointment to update.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateSelectedAppointment()
        {
            try
            {
                DataGridViewRow selectedRow = dgvAppointments.SelectedRows[0];
                int appointmentID = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);
                DateTime currentDate = Convert.ToDateTime(selectedRow.Cells["AppointmentDate"].Value);
                string notes = selectedRow.Cells["Notes"].Value?.ToString() ?? "";

                // Create update form
                using (var updateForm = new AppointmentUpdateForm(appointmentID, currentDate, notes))
                {
                    if (updateForm.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh the data
                        LoadAppointments();
                        MessageBox.Show("Appointment updated successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                DeleteSelectedAppointment();
            }
            else
            {
                MessageBox.Show("Please select an appointment to delete.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteSelectedAppointment()
        {
            try
            {
                DataGridViewRow selectedRow = dgvAppointments.SelectedRows[0];
                int appointmentID = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);
                string patientName = selectedRow.Cells["PatientName"].Value.ToString();
                DateTime appointmentDate = Convert.ToDateTime(selectedRow.Cells["AppointmentDate"].Value);

                string message = $"Are you sure you want to delete the appointment for {patientName} on {appointmentDate:MM/dd/yyyy hh:mm tt}?";
                DialogResult result = MessageBox.Show(message, "Confirm Delete", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Appointment deleted successfully!", "Success", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAppointments(); // Refresh the data
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting appointment: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper class for patient filter
        private class PatientFilterItem
        {
            public int PatientID { get; set; }
            public string PatientName { get; set; }

            public override string ToString()
            {
                return PatientName;
            }
        }
    }
} 