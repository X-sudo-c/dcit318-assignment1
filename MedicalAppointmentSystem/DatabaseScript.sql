-- Create MedicalDB Database
CREATE DATABASE MedicalDB;
GO

USE MedicalDB;
GO

-- Create Doctors table
CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Specialty VARCHAR(100) NOT NULL,
    Availability BIT DEFAULT 1
);

-- Create Patients table
CREATE TABLE Patients (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL
);

-- Create Appointments table
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorID INT NOT NULL,
    PatientID INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Notes VARCHAR(500),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID),
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID)
);

-- Insert sample data into Doctors table
INSERT INTO Doctors (FullName, Specialty, Availability) VALUES
('Dr. John Smith', 'Cardiology', 1),
('Dr. Sarah Johnson', 'Dermatology', 1),
('Dr. Michael Brown', 'Neurology', 1),
('Dr. Emily Davis', 'Pediatrics', 1),
('Dr. Robert Wilson', 'Orthopedics', 1);

-- Insert sample data into Patients table
INSERT INTO Patients (FullName, Email) VALUES
('Alice Johnson', 'alice.johnson@email.com'),
('Bob Smith', 'bob.smith@email.com'),
('Carol Davis', 'carol.davis@email.com'),
('David Wilson', 'david.wilson@email.com'),
('Eva Brown', 'eva.brown@email.com');

-- Insert sample appointments
INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) VALUES
(1, 1, '2024-01-15 10:00:00', 'Regular checkup'),
(2, 2, '2024-01-16 14:30:00', 'Skin consultation'),
(3, 3, '2024-01-17 09:00:00', 'Neurological examination');

GO 