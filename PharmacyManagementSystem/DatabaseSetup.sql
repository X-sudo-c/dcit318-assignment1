-- Create Database
CREATE DATABASE PharmacyDB;
GO

USE PharmacyDB;
GO

-- Create Medicines Table
CREATE TABLE Medicines (
    MedicineID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Quantity INT NOT NULL
);
GO

-- Create Sales Table
CREATE TABLE Sales (
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    MedicineID INT NOT NULL,
    QuantitySold INT NOT NULL,
    SaleDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MedicineID) REFERENCES Medicines(MedicineID)
);
GO

-- Create Stored Procedures

-- AddMedicine Procedure
CREATE PROCEDURE AddMedicine
    @Name VARCHAR(100),
    @Category VARCHAR(50),
    @Price DECIMAL(10,2),
    @Quantity INT
AS
BEGIN
    INSERT INTO Medicines (Name, Category, Price, Quantity)
    VALUES (@Name, @Category, @Price, @Quantity);
    
    SELECT SCOPE_IDENTITY() AS MedicineID;
END
GO

-- SearchMedicine Procedure
CREATE PROCEDURE SearchMedicine
    @SearchTerm VARCHAR(100)
AS
BEGIN
    SELECT MedicineID, Name, Category, Price, Quantity
    FROM Medicines
    WHERE Name LIKE '%' + @SearchTerm + '%'
       OR Category LIKE '%' + @SearchTerm + '%';
END
GO

-- UpdateStock Procedure
CREATE PROCEDURE UpdateStock
    @MedicineID INT,
    @Quantity INT
AS
BEGIN
    UPDATE Medicines
    SET Quantity = @Quantity
    WHERE MedicineID = @MedicineID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- RecordSale Procedure
CREATE PROCEDURE RecordSale
    @MedicineID INT,
    @QuantitySold INT
AS
BEGIN
    BEGIN TRANSACTION;
    
    -- Check if enough stock is available
    DECLARE @CurrentStock INT;
    SELECT @CurrentStock = Quantity FROM Medicines WHERE MedicineID = @MedicineID;
    
    IF @CurrentStock >= @QuantitySold
    BEGIN
        -- Insert sale record
        INSERT INTO Sales (MedicineID, QuantitySold, SaleDate)
        VALUES (@MedicineID, @QuantitySold, GETDATE());
        
        -- Update stock
        UPDATE Medicines
        SET Quantity = Quantity - @QuantitySold
        WHERE MedicineID = @MedicineID;
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT 0 AS Success;
    END
END
GO

-- GetAllMedicines Procedure
CREATE PROCEDURE GetAllMedicines
AS
BEGIN
    SELECT MedicineID, Name, Category, Price, Quantity
    FROM Medicines
    ORDER BY Name;
END
GO

-- Insert sample data
INSERT INTO Medicines (Name, Category, Price, Quantity) VALUES
('Paracetamol', 'Pain Relief', 5.99, 100),
('Amoxicillin', 'Antibiotics', 15.50, 50),
('Ibuprofen', 'Pain Relief', 7.25, 75),
('Omeprazole', 'Digestive Health', 12.99, 60),
('Cetirizine', 'Allergy Relief', 8.75, 80);
GO 