using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;

namespace MedicalAppointmentSystem
{
    public partial class DoctorListForm : Form
    {
        private DataGridView dgvDoctors;
        private Button btnRefresh;
        private Button btnClose;
        private Label lblTitle;

        public DoctorListForm()
        {
            InitializeComponent();
            LoadDoctors();
        }

        private void InitializeComponent()
        {
            this.Text = "Available Doctors";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create title label
            lblTitle = new Label
            {
                Text = "Available Doctors",
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Create DataGridView
            dgvDoctors = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
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

            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 40),
                Location = new Point(120, 10)
            };
            btnClose.Click += (sender, e) => this.Close();

            // Add controls
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Controls.Add(btnClose);

            this.Controls.Add(lblTitle);
            this.Controls.Add(dgvDoctors);
            this.Controls.Add(buttonPanel);
        }

        private void LoadDoctors()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT DoctorID, FullName, Specialty, " +
                                 "CASE WHEN Availability = 1 THEN 'Available' ELSE 'Not Available' END AS Status " +
                                 "FROM Doctors ORDER BY FullName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            dgvDoctors.DataSource = dt;

                            // Format columns
                            if (dgvDoctors.Columns.Count > 0)
                            {
                                dgvDoctors.Columns["DoctorID"].HeaderText = "Doctor ID";
                                dgvDoctors.Columns["FullName"].HeaderText = "Doctor Name";
                                dgvDoctors.Columns["Specialty"].HeaderText = "Specialty";
                                dgvDoctors.Columns["Status"].HeaderText = "Availability";

                                // Set column widths
                                dgvDoctors.Columns["DoctorID"].Width = 80;
                                dgvDoctors.Columns["FullName"].Width = 200;
                                dgvDoctors.Columns["Specialty"].Width = 150;
                                dgvDoctors.Columns["Status"].Width = 100;
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

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadDoctors();
        }
    }
} 