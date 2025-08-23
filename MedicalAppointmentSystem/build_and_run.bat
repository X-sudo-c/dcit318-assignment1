@echo off
echo Medical Appointment Booking System - Build and Run Script
echo ========================================================
echo.

echo Checking if .NET 6.0 is installed...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET 6.0 is not installed or not in PATH
    echo Please install .NET 6.0 SDK from: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo .NET 6.0 found. Building project...
echo.

echo Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

echo Building project...
dotnet build --configuration Release
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo Build successful! Running application...
echo.
echo NOTE: Make sure you have:
echo 1. SQL Server running
echo 2. MedicalDB database created (run DatabaseScript.sql)
echo 3. Connection string configured in App.config
echo.

echo Press any key to run the application...
pause >nul

echo Starting application...
dotnet run --configuration Release

echo.
echo Application closed.
pause 