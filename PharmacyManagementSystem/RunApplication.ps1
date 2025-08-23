Write-Host "Pharmacy Management System - Running Application" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Green
Write-Host ""

Write-Host "Building the application..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Build completed successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Running the application..." -ForegroundColor Yellow
    dotnet run
} else {
    Write-Host ""
    Write-Host "Build failed! Please check for errors." -ForegroundColor Red
}

Write-Host ""
Read-Host "Press Enter to continue" 