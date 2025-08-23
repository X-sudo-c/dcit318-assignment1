using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class MainForm : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PharmacyDB;Integrated Security=True;";

        public MainForm()
        {
            InitializeComponent();
            LoadAllMedicines();
        }

        private void btnAddMedicine_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || 
                    string.IsNullOrWhiteSpace(txtCategory.Text) || 
                    string.IsNullOrWhiteSpace(txtPrice.Text) || 
                    string.IsNullOrWhiteSpace(txtQuantity.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || 
                    !int.TryParse(txtQuantity.Text, out int quantity))
                {
                    MessageBox.Show("Please enter valid price and quantity values.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("AddMedicine", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        command.Parameters.AddWithValue("@Category", txtCategory.Text.Trim());
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Quantity", quantity);

                        int medicineId = Convert.ToInt32(command.ExecuteScalar());
                        MessageBox.Show($"Medicine added successfully! ID: {medicineId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        ClearInputFields();
                        LoadAllMedicines();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding medicine: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadAllMedicines();
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SearchMedicine", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SearchTerm", txtSearch.Text.Trim());

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            dgvMedicines.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching medicines: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMedicines.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a medicine to update stock.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUpdateQuantity.Text))
                {
                    MessageBox.Show("Please enter the new quantity.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtUpdateQuantity.Text, out int newQuantity))
                {
                    MessageBox.Show("Please enter a valid quantity value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int medicineId = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["MedicineID"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UpdateStock", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MedicineID", medicineId);
                        command.Parameters.AddWithValue("@Quantity", newQuantity);

                        int rowsAffected = Convert.ToInt32(command.ExecuteScalar());
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Stock updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtUpdateQuantity.Clear();
                            LoadAllMedicines();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRecordSale_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMedicines.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a medicine to record sale.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSaleQuantity.Text))
                {
                    MessageBox.Show("Please enter the quantity sold.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtSaleQuantity.Text, out int quantitySold))
                {
                    MessageBox.Show("Please enter a valid quantity value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int medicineId = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["MedicineID"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("RecordSale", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MedicineID", medicineId);
                        command.Parameters.AddWithValue("@QuantitySold", quantitySold);

                        int success = Convert.ToInt32(command.ExecuteScalar());
                        if (success == 1)
                        {
                            MessageBox.Show("Sale recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtSaleQuantity.Clear();
                            LoadAllMedicines();
                        }
                        else
                        {
                            MessageBox.Show("Insufficient stock to record sale.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error recording sale: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadAllMedicines();
            txtSearch.Clear();
        }

        private void LoadAllMedicines()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetAllMedicines", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            dgvMedicines.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading medicines: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            txtName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }
    }
} 