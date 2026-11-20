@echo off
cd /d "%~dp0"

echo Building RepairIT...
dotnet build RepairIT.sln || exit /b 1

start "RepairIT.CatalogService" dotnet run --no-build --project src\RepairIT.CatalogService
start "RepairIT.UserService" dotnet run --no-build --project src\RepairIT.UserService
start "RepairIT.RepairService" dotnet run --no-build --project src\RepairIT.RepairService
start "RepairIT.SalesService" dotnet run --no-build --project src\RepairIT.SalesService
start "RepairIT.Client" dotnet run --no-build --project src\RepairIT.Client

echo All services are starting. Client: http://localhost:5000
