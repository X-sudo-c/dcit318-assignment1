# Medical Appointment Booking System

A comprehensive Windows Forms application built with C# and ADO.NET for managing medical appointments, doctors, and patients.

## Features

### Core Functionality
- **View Available Doctors**: Display all doctors with their specialties and availability status
- **Book New Appointments**: Schedule appointments with doctors for patients
- **Manage Appointments**: View, update, and delete existing appointments
- **Patient Filtering**: Filter appointments by specific patients
- **Data Validation**: Comprehensive form validation and error handling

### Technical Features
- **ADO.NET Integration**: Uses SqlConnection, SqlCommand, SqlDataReader, and SqlDataAdapter
- **Parameterized Queries**: Secure database operations with SQL parameters
- **Exception Handling**: Comprehensive try-catch blocks with user-friendly error messages
- **Event-Driven Architecture**: Proper event handling for all form controls
- **Data Binding**: DataGridView and ComboBox data binding with DataReader and DataSet

## Database Schema

### Tables

#### Doctors
- `DoctorID` (int, Primary Key)
- `FullName` (varchar)
- `Specialty` (varchar)
- `Availability` (bit)

#### Patients
- `PatientID` (int, Primary Key)
- `FullName` (varchar)
- `Email` (varchar)

#### Appointments
- `AppointmentID` (int, Primary Key)
- `DoctorID` (int, Foreign Key)
- `PatientID` (int, Foreign Key)
- `AppointmentDate` (datetime)
- `Notes` (varchar)

## Prerequisites

- **SQL Server**: SQL Server 2019 or later (Express edition is supported)
- **.NET 6.0**: .NET 6.0 Runtime or SDK
- **Visual Studio**: Visual Studio 2022 or later (for development)

## Installation & Setup

### 1. Database Setup

1. **Open SQL Server Management Studio** or Azure Data Studio
2. **Connect to your SQL Server instance**
3. **Execute the database script**:
   - Open `DatabaseScript.sql`
   - Execute the script to create the database and sample data

### 2. Connection String Configuration

1. **Open `App.config`**
2. **Update the connection string** to match your SQL Server instance:
   ```xml
   <connectionStrings>
     <add name="MedicalDBConnection" 
          connectionString="Data Source=YOUR_SERVER;Initial Catalog=MedicalDB;Integrated Security=True;TrustServerCertificate=True"
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

   **Connection String Options**:
   - **Integrated Security**: Use Windows Authentication
   - **User ID/Password**: Use SQL Server Authentication
   - **TrustServerCertificate**: Required for SQL Server 2019+ with TLS

### 3. Build and Run

1. **Open the solution** in Visual Studio
2. **Restore NuGet packages** (if prompted)
3. **Build the solution** (Ctrl+Shift+B)
4. **Run the application** (F5)

## Usage Guide

### Main Form
The landing page provides navigation to all major features:
- **View Available Doctors**: Browse all doctors and their specialties
- **Book New Appointment**: Schedule new appointments
- **Manage Appointments**: View and modify existing appointments

### Booking Appointments
1. Select a doctor from the dropdown
2. Choose a patient
3. Pick appointment date and time
4. Add optional notes
5. Click "Book Appointment"

### Managing Appointments
1. View all appointments in the DataGridView
2. Filter by patient using the dropdown
3. Select an appointment to update or delete
4. Use the action buttons to modify data

## Technical Implementation

### ADO.NET Usage

#### Data Retrieval
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    using (SqlCommand command = new SqlCommand(query, connection))
    {
        using (SqlDataReader reader = command.ExecuteReader())
        {
            // Process data
        }
    }
}
```

#### Parameterized Commands
```csharp
command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorID;
command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientID;
command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = appointmentDate;
```

#### DataAdapter and DataSet
```csharp
SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
DataSet dataSet = new DataSet();
dataAdapter.Fill(dataSet, "Appointments");
```

### Event Handling
- **Click Events**: Button interactions
- **SelectedIndexChanged**: ComboBox selections
- **TextChanged**: TextBox input validation

### Exception Handling
```csharp
try
{
    // Database operations
}
catch (Exception ex)
{
    MessageBox.Show($"Error: {ex.Message}", "Error", 
        MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

## Troubleshooting

### Common Issues

#### Connection Errors
- Verify SQL Server is running
- Check connection string parameters
- Ensure database exists
- Verify user permissions

#### Build Errors
- Restore NuGet packages
- Check .NET version compatibility
- Verify all required files are present

#### Runtime Errors
- Check database connectivity
- Verify table structure matches schema
- Review error messages in detail

### Performance Tips
- Use appropriate indexes on frequently queried columns
- Implement connection pooling for production use
- Consider pagination for large datasets

## Security Considerations

- **SQL Injection Prevention**: All queries use parameterized commands
- **Connection Security**: Connection strings stored in configuration
- **Input Validation**: Client-side and server-side validation
- **Error Handling**: Generic error messages to prevent information disclosure

## Future Enhancements

- **User Authentication**: Login system for patients and staff
- **Appointment Reminders**: Email/SMS notifications
- **Calendar Integration**: Outlook/Google Calendar sync
- **Reporting**: Appointment statistics and analytics
- **Multi-language Support**: Internationalization
- **Mobile App**: Cross-platform companion application

## Support

For technical support or questions:
1. Check the troubleshooting section
2. Review error logs and messages
3. Verify database connectivity
4. Ensure all prerequisites are met

## License

This project is provided as-is for educational and demonstration purposes. 