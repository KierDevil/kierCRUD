#!/usr/bin/env pwsh
# Start the web application
Push-Location (Join-Path $PSScriptRoot "web\KierCRUD.Web")
dotnet run --urls "http://localhost:5173"
