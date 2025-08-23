using System;
using System.Windows.Forms;
using System.Drawing;

namespace MedicalAppointmentSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Text = "Medical Appointment Booking System";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            // Create main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Create title label
            Label titleLabel = new Label
            {
                Text = "Medical Appointment Booking System",
                Font = new Font("Arial", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            // Create navigation buttons
            Button btnViewDoctors = new Button
            {
                Text = "View Available Doctors",
                Size = new Size(250, 50),
                Font = new Font("Arial", 12),
                Tag = "Doctors"
            };
            btnViewDoctors.Click += NavigationButton_Click;

            Button btnBookAppointment = new Button
            {
                Text = "Book New Appointment",
                Size = new Size(250, 50),
                Font = new Font("Arial", 12),
                Tag = "Appointment"
            };
            btnBookAppointment.Click += NavigationButton_Click;

            Button btnManageAppointments = new Button
            {
                Text = "Manage Appointments",
                Size = new Size(250, 50),
                Font = new Font("Arial", 12),
                Tag = "Manage"
            };
            btnManageAppointments.Click += NavigationButton_Click;

            Button btnExit = new Button
            {
                Text = "Exit",
                Size = new Size(250, 50),
                Font = new Font("Arial", 12),
                BackColor = Color.LightCoral
            };
            btnExit.Click += (sender, e) => Application.Exit();

            // Create button panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Arrange buttons vertically
            int buttonY = 50;
            int spacing = 70;

            btnViewDoctors.Location = new Point(175, buttonY);
            buttonY += spacing;

            btnBookAppointment.Location = new Point(175, buttonY);
            buttonY += spacing;

            btnManageAppointments.Location = new Point(175, buttonY);
            buttonY += spacing;

            btnExit.Location = new Point(175, buttonY);

            // Add controls to panels
            buttonPanel.Controls.Add(btnViewDoctors);
            buttonPanel.Controls.Add(btnBookAppointment);
            buttonPanel.Controls.Add(btnManageAppointments);
            buttonPanel.Controls.Add(btnExit);

            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(buttonPanel);

            this.Controls.Add(mainPanel);
        }

        private void NavigationButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                Form? formToOpen = tag switch
                {
                    "Doctors" => new DoctorListForm(),
                    "Appointment" => new AppointmentForm(),
                    "Manage" => new ManageAppointmentsForm(),
                    _ => null
                };

                if (formToOpen != null)
                {
                    formToOpen.Show();
                }
            }
        }
    }
} 